using MacMarketGroupApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace MacMarketGroupApi.Services;

public class ProductsService
{
    // private readonly DBConnection _dbConnection;
    private readonly IMongoCollection<Category> _categoriesCollection;

    public ProductsService(IOptions<DBCollections> databaseSettings)
    {
        // _dbConnection = dBConnection.GetInstance();
    }

    public async void UploadNow()
    {

        // var filesUtil = new FilesUtil();
        // await filesUtil.SaveUploadedFile(images);
    }

}