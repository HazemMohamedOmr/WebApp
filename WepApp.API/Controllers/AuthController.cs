using Microsoft.AspNetCore.Mvc;
using WebApp.Core.DTOs;
using WebApp.Core.Services;
using WebApp.Infrastructure.Exceptions;

namespace WebApp.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequestDTO model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.LoginAsync(model);

        if (result.IsAuthenticated) return Ok(result);

        var problemDetails = ProblemFactory.CreateProblemDetails(HttpContext, result.Message);
        return BadRequest(problemDetails);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequestDTO model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(model);

        if (!result.IsAuthenticated)
        {
            var problemDetails = ProblemFactory.CreateProblemDetails(HttpContext, result.Message, result.Errors);
            return BadRequest(problemDetails);
        }

        return Ok(result);
    }
}