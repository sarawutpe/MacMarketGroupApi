using Microsoft.AspNetCore.Mvc;
using MacMarketGroupApi.Models;
using MacMarketGroupApi.Services;
using AutoMapper;

namespace MacMarketGroupApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
    private readonly CategoriesService _categoriesService;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ILogger<CategoriesController> logger, CategoriesService categoriesService)
    {
        _logger = logger;
        _categoriesService = categoriesService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        try
        {
            var result = await _categoriesService.GetCategories();

            MapperConfiguration config = new MapperConfiguration(cfg => cfg.CreateMap<Category, CategoryResponse>());
            var mapper = new Mapper(config);
            var data = mapper.Map<List<CategoryResponse>>(result);

            // Response
            return StatusCode(200, new Response
            {
                Success = true,
                Data = data
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
    public async Task<object> GetCategoryById(string id)
    {
        try
        {
            var result = await _categoriesService.GetCategoryById(id);

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
    public async Task<IActionResult> CreateCategory(Category category)
    {
        try
        {
            var result = await _categoriesService.CreateCategory(category);

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
    public async Task<IActionResult> UpdateCategoryById(String id, Category category)
    {
        try
        {
            var result = await _categoriesService.GetCategoryById(id);

            // Set values
            var now = DateTime.UtcNow;
            category.Id = result.Id;
            await _categoriesService.UpdateCategoryById(id, category);

            // Response
            return StatusCode(200, new Response
            {
                Success = true,
                Data = category
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
    public async Task<IActionResult> DeleteCategoryById(String id)
    {
        try
        {
            var result = await _categoriesService.GetCategoryById(id);
            await _categoriesService.DeleteCategory(id);

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
