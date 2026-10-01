using CinemaCatalog.Application.DTOs;
using CinemaCatalog.Application.Interfaces.Auth;
using CinemaCatalog.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CinemaCatalog.WebApi.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController(
    IAuthService authService,
    ISessionService sessionService) : ControllerBase
{
    private const string SessionUserKey = "auth_user";

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request, CancellationToken ct)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        try
        {
            var (user, token) = await authService.RegisterAsync(request.Login, request.Password, ct);
            var response = new AuthResponse
            {
                Id = user.Id,
                Login = user.Login,
                IdToken = token
            };

            sessionService.Set(SessionUserKey, new SessionUserDto
            {
                Id = user.Id,
                Login = user.Login
            });

            return Ok(response);
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
            var response = new AuthResponse
            {
                Id = user.Id,
                Login = user.Login,
                IdToken = token
            };

            sessionService.Set(SessionUserKey, new SessionUserDto
            {
                Id = user.Id,
                Login = user.Login
            });

            return Ok(response);
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

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        sessionService.Remove(SessionUserKey);
        return NoContent();
    }

    [HttpGet("me")]
    public ActionResult<object> Me()
    {
        var user = sessionService.Get<SessionUserDto>(SessionUserKey);
        if (user is null)
            return Unauthorized();

        return Ok(user);
    }

    private sealed class SessionUserDto
    {
        public string Id { get; set; } = "";
        public string Login { get; set; } = "";
    }
}