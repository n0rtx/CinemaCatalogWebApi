using System.Text.Json;
using CinemaCatalog.Common.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CinemaCatalog.Infrastructure.Services;

public sealed class SessionService(IHttpContextAccessor httpContextAccessor) : ISessionService
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private ISession Session =>
        httpContextAccessor.HttpContext?.Session
        ?? throw new InvalidOperationException("Session is not available. Ensure UseSession() is called.");

    public void Set<T>(string key, T value)
    {
        if (value is null)
        {
            Session.Remove(key);
            return;
        }

        var json = JsonSerializer.Serialize(value, JsonOptions);
        Session.SetString(key, json);
    }

    public T? Get<T>(string key)
    {
        var json = Session.GetString(key);
        if (string.IsNullOrEmpty(json))
            return default;

        try
        {
            return JsonSerializer.Deserialize<T>(json, JsonOptions);
        }
        catch (JsonException)
        {
            Session.Remove(key);
            return default;
        }
    }

    public void Remove(string key) => Session.Remove(key);
}