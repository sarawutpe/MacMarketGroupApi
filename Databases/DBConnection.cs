using MongoDB.Driver;

namespace MacMarketGroupApi.Services;

public class DBConnection
{
    private readonly IConfiguration _configuration;
    private readonly IMongoDatabase _database;
    private static DBConnection? _instance;

    public DBConnection(IConfiguration configuration)
    {
        _configuration = configuration;
        var connectionString = _configuration["Databases:ConnectionString"];
        var databaseName = _configuration["Databases:DatabaseName"];

        var mongoClient = new MongoClient(connectionString);
        var getDatabase = mongoClient.GetDatabase(databaseName);
        _database = getDatabase;

        if (getDatabase is null)
        {
            throw new Exception("Failed to connect to database.");
        }
    }

    public static DBConnection GetInstance(IConfiguration configuration)
    {
        if (_instance is null)
        {
            _instance = new DBConnection(configuration);
        }
        return _instance;
    }

    public IMongoCollection<T> GetCollection<T>(string collectionName)
    {
        return _database.GetCollection<T>(collectionName);
    }
}