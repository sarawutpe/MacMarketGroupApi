using Microsoft.AspNetCore.Mvc;
using MacMarketGroupApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MacMarketGroupApi.Services;

using System;

public class CustomException : Exception
{
    public int StatusCode { get; }

    public CustomException(string message, int statusCode)
        : base(message)
    {
        StatusCode = statusCode;
    }
}


public class AuthensService : Exception
{
    private readonly IOptions<DBCollections> _dbCollections;
    private readonly DBConnection _dbConnection;

    public AuthensService(IConfiguration configuration, IOptions<DBCollections> databaseSettings)
    {
        _dbCollections = databaseSettings;
        _dbConnection = DBConnection.GetInstance(configuration);
    }

    public async Task<object> Register(User user)
    {
        var collection = _dbConnection.GetCollection<User>(_dbCollections.Value.Users);

        // Check if email already exists
        var isExistEmail = await collection.Find(item => item.Email == user.Email).FirstOrDefaultAsync();
        if (isExistEmail is not null)
        {
            throw new HttpException(400, "Email already exists");
        }

        // Set timestamps
        var now = DateTime.UtcNow;
        user.CreatedAt = now;
        user.UpdatedAt = now;

        await collection.InsertOneAsync(user);
        return user;
    }

    public async Task<User> Login(Login login)
    {
        var collection = _dbConnection.GetCollection<User>(_dbCollections.Value.Users);
        var result = await collection.Find(item => item.Email == login.Email).FirstOrDefaultAsync();

        // Check if user is null
        if (result is null)
        {
            throw new HttpException(404, "Couldn’t find your Account");
        }

        // Set last accessed
        var now = DateTime.UtcNow;
        result.LastAccessed = now;
        await collection.ReplaceOneAsync(item => item.Id == result.Id, result);

        return result;
    }

}