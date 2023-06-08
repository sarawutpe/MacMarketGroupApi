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

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        try
        {
            var result = await _productsService.GetProducts();

            // Response
            return StatusCode(200, new Response
            {
                Success = true,
                Data = result
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

    [HttpGet("{id:length(24)}")]
    public async Task<IActionResult> GetProductById(String id)
    {
        try
        {
            var result = await _productsService.GetProductById(id);

            // Response
            return StatusCode(200, new Response
            {
                Success = true,
                Data = result
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

    [HttpPost]
    public async Task<IActionResult> CreateProduct(RequestCreateProduct productRequest)
    {
        try
        {
            var data = productRequest.Data;
            var images = productRequest.Images;

            // Upload file images
            List<String> fileNames = new List<string>();
            foreach (var image in images)
            {
                var file = await _filesUtil.SaveUploadedFile(image);
                if (!string.IsNullOrEmpty(file))
                {
                    fileNames.Add(file);
                }
            }

            // Added to field images 
            productRequest.Data.Images = fileNames;
            var result = await _productsService.CreateProduct(data);

            // Response
            return StatusCode(200, new Response
            {
                Success = true,
                Data = result
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

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateProductById(String id, RequestUpdateProduct requestProduct)
    {
        try
        {
            var data = requestProduct.Data;
            var images = requestProduct?.Images ?? new List<IFormFile>();

            var result = await _productsService.GetProductById(id);

            List<String> currentImageNames = result.Images;
            List<String> newImageNames = data.Images;

            var deletedImagesNames = currentImageNames.Except(newImageNames).ToList();

            // Manage file image
            // Upload new file images
            var fileNames = new List<string>();
            foreach (var image in images)
            {
                var file = await _filesUtil.SaveUploadedFile(image);
                if (!string.IsNullOrEmpty(file))
                {
                    currentImageNames.Add(file);
                }
            }

            // Delete file images
            foreach (var image in deletedImagesNames)
            {
                currentImageNames.Remove(image);
                _filesUtil.DeleteFile(image);
            }

            // Set values
            data.Id = result.Id;
            data.Images = currentImageNames;
            data.CreatedAt = result.CreatedAt;
            data.UpdatedAt = result.UpdatedAt;
            await _productsService.UpdateProductById(id, data);

            // Response
            return StatusCode(200, new Response
            {
                Success = true,
                Data = result
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

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteProductById(String id)
    {
        try
        {
            var result = await _productsService.GetProductById(id);
            await _productsService.DeleteProduct(id);

            // Delete file images
            result.Images.Select(image => _filesUtil.DeleteFile(image));

            // Response
            return StatusCode(200, new Response
            {
                Success = true,
                Data = result
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
