using MongoDB.Driver;
using WorkerService.Models;

namespace WorkerService.Repositories;

public class MongoProductRepository : IProductRepository
{
    private readonly IMongoCollection<Product> _collection;


    public MongoProductRepository(IMongoDatabase database)
    {
        _collection = database.GetCollection<Product>("products");
    }

    public async Task<Product> GetByIdAsync(string id)
    {
        return await _collection.Find(p => p.Id == id).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(Product product)
    {
        await _collection.InsertOneAsync(product);
    }
}
