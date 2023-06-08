using MacMarketGroupApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MacMarketGroupApi.Services;

public class CategoriesService
{
    private readonly IOptions<DBCollections> _dbCollections;
    private readonly DBConnection _dbConnection;

    public CategoriesService(IConfiguration configuration, IOptions<DBCollections> databaseSettings)
    {
        _dbCollections = databaseSettings;
        _dbConnection = DBConnection.GetInstance(configuration);
    }

    public async Task<List<Category>> GetCategories()
    {
        var collection = _dbConnection.GetCollection<Category>(_dbCollections.Value.Categories);
        var sortDefinition = Builders<Category>.Sort.Descending(category => category.Id);
        var result = await collection.Find(_ => true).Sort(sortDefinition).ToListAsync();
        return result;
    }

    public async Task<Category> GetCategoryById(String id)
    {
        var collection = _dbConnection.GetCollection<Category>(_dbCollections.Value.Categories);
        var result = await collection.Find(category => category.Id == id).FirstOrDefaultAsync();

        if (result is null)
        {
            throw new HttpException(404, "Not Found");
        }
        return result;
    }

    public async Task<Category> CreateCategory(Category category)
    {
        var collection = _dbConnection.GetCollection<Category>(_dbCollections.Value.Categories);

        // Set timestamps
        var now = DateTime.UtcNow;
        category.CreatedAt = now;
        category.UpdatedAt = now;

        await collection.InsertOneAsync(category);
        return category;
    }

    public async Task<bool> UpdateCategoryById(String id, Category category)
    {

        // Set timestamps
        var now = DateTime.UtcNow;
        category.UpdatedAt = now;

        var collection = _dbConnection.GetCollection<Category>(_dbCollections.Value.Categories);
        var result = await collection.ReplaceOneAsync(category => category.Id == id, category);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteCategory(String id)
    {
        var collection = _dbConnection.GetCollection<Category>(_dbCollections.Value.Categories);
        var result = await collection.DeleteOneAsync(category => category.Id == id);
        return result.DeletedCount > 0;
    }
}