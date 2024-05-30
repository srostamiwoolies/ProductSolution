using Microsoft.AspNetCore.Mvc;
using Service1.Services;

namespace Service1.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]

    public async Task<IEnumerable<Models.Product>> GetAll()
    {
        return await _productService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<Models.Product> Get(string id)
    {
        return await _productService.GetByIdAsync(id);
    }

    [HttpPost]
    public async Task<ActionResult<Models.Product>> CreateProduct(Models.ProductDto productdto)
    {
        var product = new Models.Product { Name = productdto.Name, Price = productdto.Price };

        await _productService.CreateAsync(product);

        return Created();
    }
}