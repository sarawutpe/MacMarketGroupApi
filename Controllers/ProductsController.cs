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


    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductRequest productRequest)
    {
        try
        {
            var data = productRequest.Data;
            var images = productRequest.Images;

            // Upload file images
            var fileNames = images.Select(async image => await _filesUtil.SaveUploadedFile(image))
                      .Where(fileNameTask => fileNameTask.Result != "")
                      .Select(fileNameTask => fileNameTask.Result)
                      .ToList();

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
