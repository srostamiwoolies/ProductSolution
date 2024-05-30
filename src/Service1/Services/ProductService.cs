using Service1.Models;
using Service1.Repositories;
using StackExchange.Redis;
using System.Text.Json;

namespace Service1.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly IDatabase _redisDatabase;
    private readonly string _redisProductHashName;

    public ProductService(IProductRepository repository, IConnectionMultiplexer multiplexer)
    {
        _repository = repository;
        _redisDatabase = multiplexer.GetDatabase();
        _redisProductHashName = "product";
    }

    public async Task<Product> GetByIdAsync(string id)
    {
        var key = $"{id}";
        var json = await _redisDatabase.HashGetAsync(_redisProductHashName, key);

        if (json.HasValue)
        {
            var productFromRedis = JsonSerializer.Deserialize<Product>(json!);

            return productFromRedis;
        }

        var productFromDb = await _repository.GetByIdAsync(id);

        string serializedProduct = JsonSerializer.Serialize(productFromDb);

        await _redisDatabase.HashSetAsync(_redisProductHashName, key, serializedProduct);

        return productFromDb;
    }

    public async Task CreateAsync(Product product)
    {
        await _repository.CreateAsync(product);
    }
}