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
        string? settingFilePath = _configuration["AppSettings:FilePath"];
        if (settingFilePath is null)
        {
            throw new InvalidOperationException("Setting path not found.");
        }

        // Check error 
        if (!allowedContentTypes.Contains(file.ContentType))
        {
            throw new ArgumentException($"Allowed are {allowedContentTypesString} only!.");
        }

        // Generate a unique file name or use any desired naming convention
        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);

        // Copy file
        string streamPath = Path.Combine(settingFilePath, fileName);
        using (var stream = new FileStream(streamPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return fileName;
    }

    public bool DeleteFile(string filePath)
    {
        // Set the path where you want to save the image
        string? settingFilePath = _configuration["AppSettings:FilePath"];
        if (settingFilePath is null)
        {
            throw new InvalidOperationException("Setting path not found.");
        }

        var path = $"{settingFilePath}{filePath}";
        if (File.Exists(path))
        {
            File.Delete(path);
            return true;
        }

        return false;
    }

}
