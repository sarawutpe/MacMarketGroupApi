using Microsoft.AspNetCore.Mvc;
using MacMarketGroupApi.Models;
using MacMarketGroupApi.Services;
using AutoMapper;

namespace MacMarketGroupApi.Controllers;
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly UsersService _usersService;

    public UsersController(ILogger<AuthensController> logger, UsersService usersService)
    {
        _usersService = usersService;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        try
        {
            var result = await _usersService.GetUsers();

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
    public async Task<IActionResult> GetUserById(string id)
    {
        try
        {
            var result = await _usersService.GetUserById(id);

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