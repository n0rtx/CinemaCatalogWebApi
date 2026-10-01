using CinemaCatalog.Common.Interfaces;
using CinemaCatalog.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace CinemaCatalog.WebApi.Controllers;

[ApiController]
[Route("api/theme")]
public class ThemeController(ICookieService cookieService) : ControllerBase
{
    private const string ThemeCookieName = "cinema_theme";

    [HttpGet]
    public ActionResult<object> GetTheme()
    {
        var theme = cookieService.Get(ThemeCookieName);
        if (theme is not ("dark" or "light"))
            theme = "dark";

        return Ok(new { theme });
    }

    [HttpPost]
    public IActionResult SetTheme([FromBody] SetThemeRequest request)
    {
        if (request.Theme is not ("dark" or "light"))
            return BadRequest("Theme must be 'dark' or 'light'.");

        cookieService.Set(ThemeCookieName, request.Theme, CookieService.ThemeOptions());
        return NoContent();
    }

    public sealed class SetThemeRequest
    {
        public string Theme { get; set; } = "dark";
    }
}