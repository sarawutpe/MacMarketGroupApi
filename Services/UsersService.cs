using MacMarketGroupApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MacMarketGroupApi.Services;

public class UsersService
{
    private readonly IOptions<DBCollections> _dbCollections;
    private readonly DBConnection _dbConnection;

    public UsersService(IConfiguration configuration, IOptions<DBCollections> databaseSettings)
    {
        _dbCollections = databaseSettings;
        _dbConnection = DBConnection.GetInstance(configuration);
    }


    public async Task<List<User>> GetUsers()
    {
        var collection = _dbConnection.GetCollection<User>(_dbCollections.Value.Users);
        var result = await collection.Find(_ => true).ToListAsync();
        return result;
    }

    public async Task<User> GetUserById(String id)
    {
        var collection = _dbConnection.GetCollection<User>(_dbCollections.Value.Users);
        var result = await collection.Find(item => item.Id == id).FirstOrDefaultAsync();

        if (result is null)
        {
            throw new HttpException(404, "Not Found");
        }
        return result;
    }
}