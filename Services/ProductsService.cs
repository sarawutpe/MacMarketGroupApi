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
        // Aggregation pipelines
        var pipeline = new BsonDocument[]
        {
            new BsonDocument("$match", new BsonDocument
            {
                { "$or", new BsonArray
                    {
                        new BsonDocument("name", new BsonDocument("$regex", new BsonRegularExpression(search, "i")))
                    }
                }
            }),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "categories" },
                { "localField", "categoryId" },
                { "foreignField", "_id" },
                { "as", "category" }
            }),
            new BsonDocument("$unwind", "$category"),
            new BsonDocument("$sort", new BsonDocument("_id", -1)),
            new BsonDocument("$skip", (pageNumber - 1) * pageSize),
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

    public async Task<Response> GetProductsByUserId(string userId, int pageNumber, int pageSize, string search)
    {
        var id = new ObjectId(userId);

        // Aggregation pipelines
        var pipeline = new BsonDocument[]
        {
            new BsonDocument("$match", new BsonDocument
            {
                { "userId", id },
                { "$or", new BsonArray
                    {
                        new BsonDocument("name", new BsonDocument("$regex", new BsonRegularExpression(search, "i")))
                    }
                }
            }),
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "categories" },
                { "localField", "categoryId" },
                { "foreignField", "_id" },
                { "as", "category" }
            }),
            new BsonDocument("$unwind", "$category"),
            new BsonDocument("$sort", new BsonDocument("_id", -1)),
            new BsonDocument("$skip", (pageNumber - 1) * pageSize),
            new BsonDocument("$limit", pageSize),
        };

        var collection = _dbConnection.GetCollection<Product>(_dbCollections.Value.Products);
        var total = await collection.CountDocumentsAsync(Builders<Product>.Filter.Eq("userId", id));
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

    public async Task<Response> GetProductsByCategoryCode(string categoryCode, int pageNumber, int pageSize, string search)
    {
        // Aggregation pipelines
        var pipeline = new BsonDocument[]
        {
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "categories" },
                { "localField", "categoryId" },
                { "foreignField", "_id" },
                { "as", "category" }
            }),
            new BsonDocument("$match", new BsonDocument
            {
                {"category.code", new BsonDocument("$regex", new BsonRegularExpression(categoryCode, "i"))},
                { "$or", new BsonArray
                    {
                        new BsonDocument("name", new BsonDocument("$regex", new BsonRegularExpression(search, "i"))),
                    }
                }
            }),
            new BsonDocument("$unwind", "$category"),
            new BsonDocument("$sort", new BsonDocument("_id", -1)),
            new BsonDocument("$skip", (pageNumber - 1) * pageSize),
            new BsonDocument("$limit", pageSize),
        };

        var collection = _dbConnection.GetCollection<Product>(_dbCollections.Value.Products);

        var pipelineWithCount = new BsonDocument[]
        {
            new BsonDocument("$lookup", new BsonDocument
            {
                { "from", "categories" },
                { "localField", "categoryId" },
                { "foreignField", "_id" },
                { "as", "category" }
            }),
            new BsonDocument("$match", new BsonDocument
            {
                {"category.code", new BsonDocument("$regex", new BsonRegularExpression(categoryCode, "i"))},
            }),
             new BsonDocument("$unwind", "$category"),
        };
        var total = (await collection.Aggregate<Product>(pipelineWithCount).ToListAsync()).Count();
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