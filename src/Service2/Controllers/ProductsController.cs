using Microsoft.AspNetCore.Mvc;
using Service2.Models;

namespace Service2.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly HttpClient _httpClient;

    public ProductsController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("Service1");
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
            var content = await response.Content.ReadAsStringAsync();
            return Ok(content);
        }

        return StatusCode((int)response.StatusCode);
    }
}
