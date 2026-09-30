using CinemaCatalog.Application.DTOs;
using CinemaCatalog.Application.Interfaces;
using CinemaCatalog.Application.Interfaces.Auth;
using Microsoft.AspNetCore.Mvc;

namespace CinemaCatalog.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var (user, token) = await authService.RegisterAsync(request.Login, request.Password, ct);
            return Ok(new AuthResponse
            {
                Id = user.Id,
                Login = user.Login,
                IdToken = token
            });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var (user, token) = await authService.LoginAsync(request.Login, request.Password, ct);
            return Ok(new AuthResponse
            {
                Id = user.Id,
                Login = user.Login,
                IdToken = token
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized("Invalid login or password");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}