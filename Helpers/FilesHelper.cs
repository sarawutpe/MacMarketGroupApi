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

    public async Task<List<string>> SaveUploadedFile(IEnumerable<IFormFile> files)
    {
        List<string> fileNames = new List<string>();
        List<string> allowedContentTypes = new List<string>
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

        foreach (var file in files)
        {
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

            fileNames.Add(fileName);
        }

        return fileNames;
    }

    // Parameter is String, List String
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
        // Check if the passed value is an enumerable collection of file paths
        else if (filePaths is IEnumerable<string> multipleFilePaths)
        {
            foreach (string filePath in multipleFilePaths)
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    deletedCount++;
                }
            }
        }

        return $"{deletedCount} entries deleted.";
    }

}
