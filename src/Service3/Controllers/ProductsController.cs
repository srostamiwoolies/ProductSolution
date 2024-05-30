using Microsoft.AspNetCore.Mvc;
using Service3.Models;
using Service3.Services;
using System.Text.Json;

namespace Service3.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly HttpClient _httpClient;
    private readonly IMessageService _messageService;

    public ProductsController(IHttpClientFactory httpClientFactory, IMessageService messageService)
    {
        _httpClient = httpClientFactory.CreateClient("Service2");
        _messageService = messageService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _httpClient.GetAsync($"/api/products");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        return StatusCode((int)response.StatusCode);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProducts(string id)
    {
        var response = await _httpClient.GetAsync($"/api/products/{id}");

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        return StatusCode((int)response.StatusCode);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] ProductDto product)
    {
        var response = await _httpClient.PostAsJsonAsync("/api/products", product);

        if (response.IsSuccessStatusCode)
        {
            await _messageService.Send(JsonSerializer.Serialize(product));

            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        return StatusCode((int)response.StatusCode);
    }
}
