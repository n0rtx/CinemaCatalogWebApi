using CinemaCatalog.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CinemaCatalog.Infrastructure.Services;

public sealed class CookieService(IHttpContextAccessor httpContextAccessor) : ICookieService
{
    private HttpContext HttpContext =>
        httpContextAccessor.HttpContext
        ?? throw new InvalidOperationException("HttpContext is not available.");

    public void Set(string key, string value, CookieOptions? options)
    {
        options ??= DefaultOptions();
        options.Secure = HttpContext.Request.IsHttps;
        HttpContext.Response.Cookies.Append(key, value, options);
    }

    public string? Get(string key)
    {
        return HttpContext.Request.Cookies.TryGetValue(key, out var value)
            ? value
            : null;
    }

    public void Remove(string key)
    {
        HttpContext.Response.Cookies.Delete(key, new CookieOptions
        {
            Path = "/",
            SameSite = SameSiteMode.Lax,
            Secure = HttpContext.Request.IsHttps
        });
    }

    public static CookieOptions DefaultOptions(TimeSpan? maxAge = null) => new()
    {
        Path = "/",
        HttpOnly = true,
        SameSite = SameSiteMode.Lax,
        MaxAge = maxAge ?? TimeSpan.FromDays(7),
        IsEssential = true
    };
    
    public static CookieOptions ThemeOptions() => new()
    {
        Path = "/",
        HttpOnly = false,
        SameSite = SameSiteMode.Lax,
        MaxAge = TimeSpan.FromDays(365),
        IsEssential = true
    };
}