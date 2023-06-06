using Microsoft.AspNetCore.Mvc;
using MacMarketGroupApi.Services;
namespace MacMarketGroupApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoresController : ControllerBase
{
    private readonly AuthsHelper _jwtService;
    private readonly ILogger<CategoriesController> _logger;

    public CoresController(ILogger<CategoriesController> logger, AuthsHelper jwtService)
    {
        _logger = logger;
        _jwtService = jwtService;
    }

    [HttpGet(Name = "Cores")]
    public object GetCore()
    {
        // Authenticate user and get the userId
        // Generate JWT token
        // string demotoken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjEyMzQiLCJleHAiOjE2ODQ4NzAxNTUsImlzcyI6IllvdXJJc3N1ZXIiLCJhdWQiOiJZb3VyQXVkaWVuY2UifQ.f1aZxrNFxou5jVHwoP_RADsns2BbI20QmJU4oiApang";
        // var token = _jwtService.GenerateToken("1234567858181");
        var jwtv = _jwtService.GenerateToken("1234567858181");
        // string color = ColorsUtil.GenerateRandomHexColor();
        return jwtv;
    }

}
