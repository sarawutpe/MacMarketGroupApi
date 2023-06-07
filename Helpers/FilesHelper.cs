using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using MacMarketGroupApi.Models;

namespace MacMarketGroupApi.Services;
public class FilesHelper
{
    private readonly IConfiguration _configuration;
    public FilesHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<string> SaveUploadedFile(IFormFile file)
    {
        var allowedContentTypes = new List<string>
        {
            "image/jpeg",
            "image/png",
            "image/gif",
            "image/bmp",
            "image/tiff",
            "image/svg+xml",
            "image/webp"
        };
        string allowedContentTypesString = string.Join(", ", allowedContentTypes);

        // Set the path where you want to save the image
        string? path = _configuration["AppSettings:FilePath"];
        if (path is null)
        {
            throw new InvalidOperationException("Path not found.");
        }

        // Check error 
        if (!allowedContentTypes.Contains(file.ContentType))
        {
            throw new ArgumentException($"Allowed are {allowedContentTypesString} only!.");
        }

        // Generate a unique file name or use any desired naming convention
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        // Copy file
        string filePath = Path.Combine(path, fileName);
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return fileName;
    }

    public string DeleteFile(object filePaths)
    {
        int deletedCount = 0;

        // Check if the passed value is a single file path as a string
        if (filePaths is string singleFilePath)
        {
            if (File.Exists(singleFilePath))
            {
                File.Delete(singleFilePath);
                deletedCount++;
            }
        }

        return $"{deletedCount} entries deleted.";
    }

}
