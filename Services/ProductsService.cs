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

    public async Task<List<Product>> GetProducts()
    {
        var collection = _dbConnection.GetCollection<Product>(_dbCollections.Value.Products);
        var sortDefinition = Builders<Product>.Sort.Descending(product => product.Id);
        var result = await collection.Find(_ => true).Sort(sortDefinition).ToListAsync();
        return result;
    }

    public async Task<Product> GetProductById(String id)
    {
        var collection = _dbConnection.GetCollection<Product>(_dbCollections.Value.Products);
        var result = await collection.Find(product => product.Id == id).FirstOrDefaultAsync();

        if (result is null)
        {
            throw new HttpException(404, "Not Found");
        }

        return result;
    }

    public async Task<Product> CreateProduct(Product product)
    {
        // Set timestamps
        var now = DateTime.UtcNow;
        product.CreatedAt = now;
        product.UpdatedAt = now;

        var collection = _dbConnection.GetCollection<Product>(_dbCollections.Value.Products);
        await collection.InsertOneAsync(product);
        return product;
    }

    public async Task<bool> UpdateProductById(String id, Product product)
    {
        // Set timestamps
        var now = DateTime.UtcNow;
        product.UpdatedAt = now;

        var collection = _dbConnection.GetCollection<Product>(_dbCollections.Value.Products);
        var result = await collection.ReplaceOneAsync(product => product.Id == id, product);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteProduct(String id)
    {
        var collection = _dbConnection.GetCollection<Product>(_dbCollections.Value.Products);
        var result = await collection.DeleteOneAsync(product => product.Id == id);
        return result.DeletedCount > 0;
    }
}