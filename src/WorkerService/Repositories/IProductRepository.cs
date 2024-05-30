using WorkerService.Models;

namespace WorkerService.Repositories;

public interface IProductRepository
{
    Task<Product> GetByIdAsync(string id);

    Task CreateAsync(Product product);
}
