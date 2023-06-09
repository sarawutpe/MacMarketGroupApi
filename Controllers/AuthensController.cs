using Microsoft.AspNetCore.Mvc;
using MacMarketGroupApi.Models;
using MacMarketGroupApi.Services;
using AutoMapper;
using BCryptNet = BCrypt.Net;
using Newtonsoft.Json;

namespace MacMarketGroupApi.Controllers;
[ApiController]
[Route("api/authens")]
public class AuthensController : ControllerBase
{
    private readonly AuthensService _authensService;
    private readonly UsersService _usersService;

    public AuthensController(ILogger<AuthensController> logger, AuthensService authensService, UsersService usersService)
    {
        _authensService = authensService;
        _usersService = usersService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        try
        {
            // Set values
            // Hash a password
            user.Password = BCryptNet.BCrypt.HashPassword(user.Password);
            var result = await _authensService.Register(user);

            // Response
            return StatusCode(200, new Response
            {
                Success = true,
                Data = result,
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

    [HttpPost("login")]
    public async Task<object> Login(Login login)
    {
        try
        {
            var result = await _authensService.Login(login);

            bool isMatched = BCryptNet.BCrypt.Verify(login.Password, result.Password);
            if (!isMatched)
            {
                return StatusCode(404, new Response
                {
                    Success = false,
                    Error = "Incorrect email or password"
                });
            }

            // Response
            return StatusCode(200, new Response
            {
                Success = true,
                Data = result,
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

    [HttpPost("forgot-password")]
    public async Task<object> ForgotPassword(RequestForgotPassword forgotPassword)
    {
        try
        {
            var users = await _usersService.GetUsers();
            var user = users.Find(item => item.Email == forgotPassword.Email);

            if (user is null)
            {
                return StatusCode(404, new Response
                {
                    Success = false,
                    Data = "Not Found",
                });
            }

            // Response
            return StatusCode(200, new Response
            {
                Success = true,
                Data = "Token is 4484844e",
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
