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

    public async Task<List<Category>> GetCategory()
    {
        var collection = _dbConnection.GetCollection<Category>(_dbCollections.Value.Categories);
        var result = await collection.Find(_ => true).ToListAsync();
        return result;
    }

    public async Task<Category> GetCategoryById(String id)
    {
        var collection = _dbConnection.GetCollection<Category>(_dbCollections.Value.Categories);
        var result = await collection.Find(item => item.Id == id).FirstOrDefaultAsync();

        if (result is null)
        {
            throw new HttpException(404, "Not Found");
        }
        return result;
    }

    public async Task<Category> CreateCategory(Category category)
    {
        var collection = _dbConnection.GetCollection<Category>(_dbCollections.Value.Categories);
        await collection.InsertOneAsync(category);
        return category;
    }

    public async Task<bool> UpdateCategoryById(String id, Category category)
    {
        var collection = _dbConnection.GetCollection<Category>(_dbCollections.Value.Categories);
        var result = await collection.ReplaceOneAsync(item => item.Id == id, category);
        return result.ModifiedCount > 0;
    }

    public async Task<bool> DeleteCategory(String id)
    {
        var collection = _dbConnection.GetCollection<Category>(_dbCollections.Value.Categories);
        var result = await collection.DeleteOneAsync(item => item.Id == id);
        return result.DeletedCount > 0;
    }
}