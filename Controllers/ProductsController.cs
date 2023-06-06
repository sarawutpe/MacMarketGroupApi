using Microsoft.AspNetCore.Mvc;
using MacMarketGroupApi.Services;

namespace MacMarketGroupApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly ProductsService _productsService;
    private readonly FilesHelper _filesUtil;
    private readonly ILogger<CategoriesController> _logger;

    public ProductsController(ILogger<CategoriesController> logger, ProductsService productsService, FilesHelper filesUtil)
    {
        _logger = logger;
        _productsService = productsService;
        _filesUtil = filesUtil;
    }

    [HttpGet(Name = "Product")]
    public String Get()
    {
        return "Hello dotnet";
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(List<IFormFile> images)
    {

        _logger.LogInformation("Hello C#");
        var result = await _filesUtil.SaveUploadedFile(images);
        Console.WriteLine(result);

        string filePath = "path/to/file.ext";
        _filesUtil.DeleteFile(filePath);

        List<string> filePaths = new List<string>
        {
            "path/to/file1.ext",
            "path/to/file2.ext",
        };
        _filesUtil.DeleteFile(filePaths);

        // _productsService.UploadNow();
        // if (imageFile == null || imageFile.Length <= 0)
        //     return BadRequest("No file uploaded.");

        // // Generate a unique file name or use any desired naming convention
        // string fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);

        // // Set the path where you want to save the image
        // string filePath = Path.Combine("uploads/", fileName);

        // using (var stream = new FileStream(filePath, FileMode.Create))
        // {
        //     await imageFile.CopyToAsync(stream);
        // }


        // var filesUtil = new FilesUtil();
        // await filesUtil.SaveUploadedFile(images);
        // var newFileName = await FilesUtil.SaveUploadedFile(images);

        var newFileName = "";
        // Optionally, you can return a response with the saved file path or any other relevant information
        return Ok($"Image uploaded successfully. File path: {newFileName}");
    }


}
