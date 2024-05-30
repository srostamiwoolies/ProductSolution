using Service1.Models;

namespace Service1.Services;

public interface IProductService
{

    Task<Product> GetByIdAsync(string id);

    Task CreateAsync(Product product);

    Task<List<Product>> GetAllAsync();
}
