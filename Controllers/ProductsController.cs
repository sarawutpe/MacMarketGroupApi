using Microsoft.AspNetCore.Mvc;
using MacMarketGroupApi.Models;
using MacMarketGroupApi.Services;
using AutoMapper;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace MacMarketGroupApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductsService _productsService;
    private readonly FilesHelper _filesUtil;

    public ProductsController(ILogger<CategoriesController> logger, ProductsService productsService, FilesHelper filesUtil)
    {
        _productsService = productsService;
        _filesUtil = filesUtil;
    }

    [HttpGet(Name = "Product")]
    public String Get()
    {
        return "Hello dotnet";
    }

    // [HttpPost]
    // public async Task<IActionResult> UploadImage(List<IFormFile> images)
    // {

    //     var result = await _filesUtil.SaveUploadedFile(images);
    //     Console.WriteLine(result);

    //     string filePath = "path/to/file.ext";
    //     _filesUtil.DeleteFile(filePath);

    //     List<string> filePaths = new List<string>
    //     {
    //         "path/to/file1.ext",
    //         "path/to/file2.ext",
    //     };
    //     _filesUtil.DeleteFile(filePaths);

    //     // _productsService.UploadNow();
    //     // if (imageFile == null || imageFile.Length <= 0)
    //     //     return BadRequest("No file uploaded.");

    //     // // Generate a unique file name or use any desired naming convention
    //     // string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

    //     // // Set the path where you want to save the image
    //     // string filePath = Path.Combine("uploads/", fileName);

    //     // using (var stream = new FileStream(filePath, FileMode.Create))
    //     // {
    //     //     await imageFile.CopyToAsync(stream);
    //     // }


    //     // var filesUtil = new FilesUtil();
    //     // await filesUtil.SaveUploadedFile(images);
    //     // var newFileName = await FilesUtil.SaveUploadedFile(images);

    //     var newFileName = "";
    //     // Optionally, you can return a response with the saved file path or any other relevant information
    //     return Ok($"Image uploaded successfully. File path: {newFileName}");
    // }

    [HttpPost]
    // [ServiceFilter(typeof(CustomActionFilter))]
    public async Task<IActionResult> CreateProduct(ProductRequest productRequest)
    {
        try
        {

            if (ModelState.IsValid){}

            // var newProduct = new Product
            // {
            //     UserId = "9876543210",
            //     CategoryId = "5432109876",
            //     Name = "Sample Product",
            //     Images = new[] { "image1.jpg", "image2.jpg", "image3.jpg" },
            //     Price = 99.99m,
            //     Condition = Condition.LIKE_NEW,
            //     Description = "This is a sample product description.",
            //     Location = "Sample Location",
            //     IsPublic = true,
            //     IsActive = true,
            //     CreatedAt = DateTime.Now,
            //     UpdatedAt = DateTime.Now
            // };

            // var productReq = JsonSerializer.Deserialize<Product>(productRequest.Data);
            // var validationContext = new ValidationContext(newProduct);
            // var validationResults = new List<ValidationResult>();

            // if (!Validator.TryValidateObject(productReq, validationContext, validationResults, true))
            // {
            //     var validationErrors = validationResults.Select(result => result.ErrorMessage);
            //     return BadRequest(validationErrors);
            // }

            // var user = new Login { Email = "admin@gmail.com", Password = "1234" };
            // var options = new JsonSerializerOptions { WriteIndented = true };
            // var jsonString = JsonSerializer.Serialize(user);

            // '{"username":"admin","password":"1234"}'

            // var userObj = JsonSerializer.Deserialize<Login>("{\"Email\":\"admin@gmail.com\",\"Password\":\"1234\"}");

            // await _productsService.CreateProduct(productRequest);



            // var productStr = JsonSerializer.Serialize(product);

            // var product = JsonSerializer.Deserialize<Product>(productRequest.Data);
            // var validationContext = new ValidationContext(product);
            // var validationResults = new List<ValidationResult>();
            // bool isValid = Validator.TryValidateObject(product, validationContext, validationResults, true);


            // Response
            return StatusCode(200, new Dictionary<string, object>
            {
                {"data", productRequest.Data},
            });
        }
        catch (HttpException exception)
        {
            return StatusCode(exception.StatusCode, new Response
            {
                Success = false,
                Error = exception.Message
            });
        }
    }


}
