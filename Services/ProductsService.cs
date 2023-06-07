using MacMarketGroupApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MacMarketGroupApi.Services;

public class ProductsService : Exception
{
    private readonly IOptions<DBCollections> _dbCollections;
    private readonly DBConnection _dbConnection;

    public ProductsService(IConfiguration configuration, IOptions<DBCollections> databaseSettings)
    {
        _dbCollections = databaseSettings;
        _dbConnection = DBConnection.GetInstance(configuration);
    }


    public async Task<object> CreateProduct(ProductRequest productRequest)
    {
        var collection = _dbConnection.GetCollection<User>(_dbCollections.Value.Products);
        return await collection.Find(_ => true).ToListAsync();
    }

}