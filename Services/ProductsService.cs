using MacMarketGroupApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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


    public async Task<Response> GetProducts(int pageNumber, int pageSize, string search)
    {
        // Pagination
        var searchDefinition = new BsonDocument();
        if (!string.IsNullOrEmpty(search))
        {
            searchDefinition.Add("name", new BsonDocument("$regex", new BsonRegularExpression(search, "i")));
        }
        var sortDefinition = new BsonDocument("_id", -1);
        var skipDefinition = (pageNumber - 1) * pageSize;
        var pipeline = new BsonDocument[]
        {
            new BsonDocument("$match", new BsonDocument()),
            new BsonDocument{
                {
                    "$lookup", new BsonDocument{
                        { "from", "categories" },
                        { "localField", "categoryId" },
                        { "foreignField", "_id" },
                        { "as", "category" }
                    }
                }
            },
            new BsonDocument("$unwind", "$category"),
            new BsonDocument("$match", searchDefinition),
            new BsonDocument("$sort", sortDefinition),
            new BsonDocument("$skip", skipDefinition),
            new BsonDocument("$limit", pageSize),
        };

        var collection = _dbConnection.GetCollection<Product>(_dbCollections.Value.Products);
        var total = await collection.CountDocumentsAsync(new BsonDocument());
        var result = await collection.Aggregate<Product>(pipeline).ToListAsync();

        return new Response
        {
            Success = true,
            Total = total,
            PageSize = pageSize,
            PageNumber = pageNumber,
            Data = result
        };
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