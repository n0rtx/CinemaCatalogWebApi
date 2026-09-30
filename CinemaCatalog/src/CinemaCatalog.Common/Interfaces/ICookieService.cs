using Microsoft.AspNetCore.Http;

namespace CinemaCatalog.Common.Interfaces;

public interface ICookieService
{
    void Set(string key, string value, CookieOptions? options);
    
    string? Get(string key);
    
    void Remove(string key);
}