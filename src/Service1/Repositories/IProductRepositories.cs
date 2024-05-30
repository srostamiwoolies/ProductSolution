using Service1.Models;

namespace Service1.Repositories;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(string id);

    Task CreateAsync(Product product);

    Task<List<Product>> GetAllAsync();
}
