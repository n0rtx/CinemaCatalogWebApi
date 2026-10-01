using System.Net.Http.Json;
using CinemaCatalog.Application.Interfaces.Auth;
using CinemaCatalog.Domain.Entities;
using CinemaCatalog.Domain.Interfaces;
using FirebaseAdmin.Auth;
using Microsoft.Extensions.Configuration;

namespace CinemaCatalog.Infrastructure.Services;

public class FirebaseAuthService : IAuthService
{
    private readonly IUserRepository _userRepository;
    private readonly string _webApiKey;

    public FirebaseAuthService(IUserRepository userRepository, IConfiguration configuration)
    {
        _userRepository = userRepository;
        _webApiKey = configuration["Firebase:WebApiKey"]
                     ?? Environment.GetEnvironmentVariable("FIREBASE_WEB_API_KEY")
                     ?? throw new InvalidOperationException(
                         "Firebase:WebApiKey is missing (appsettings or FIREBASE_WEB_API_KEY)");
    }

    public async Task<(User User, string IdToken)> RegisterAsync(string login, string password, CancellationToken ct)
    {
        if (await _userRepository.GetByLoginAsync(login, ct) is not null)
            throw new InvalidOperationException("User with this login already exists");

        var email = login.Contains('@') ? login : $"{login}@cinemacatalog.local";

        var userRecord = await FirebaseAuth.DefaultInstance.CreateUserAsync(new UserRecordArgs
        {
            Email = email,
            Password = password,
            DisplayName = login,
            EmailVerified = false
        }, ct);

        var user = new User
        {
            Id = userRecord.Uid,
            Login = login,
            CreatedAt = DateTime.UtcNow
        };

        await _userRepository.AddAsync(user, ct);

        var customToken = await FirebaseAuth.DefaultInstance.CreateCustomTokenAsync(userRecord.Uid);
        return (user, customToken);
    }

    public async Task<(User User, string IdToken)> LoginAsync(string login, string password, CancellationToken ct)
    {
        var email = login.Contains('@') ? login : $"{login}@cinemacatalog.local";
        var idToken = await SignInWithPasswordAsync(email, password, ct);

        var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(idToken, ct);
        var user = await _userRepository.GetByIdAsync(decoded.Uid, ct)
                   ?? throw new UnauthorizedAccessException("User profile not found");

        return (user, idToken);
    }

    private async Task<string> SignInWithPasswordAsync(string email, string password, CancellationToken ct)
    {
        using var http = new HttpClient();
        var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={_webApiKey}";
        var body = new { email, password, returnSecureToken = true };

        var response = await http.PostAsJsonAsync(url, body, ct);

        if (!response.IsSuccessStatusCode)
            throw new UnauthorizedAccessException("Invalid login or password");

        var json = await response.Content.ReadFromJsonAsync<SignInResponse>(cancellationToken: ct)
                   ?? throw new UnauthorizedAccessException("Invalid credentials");

        return json.IdToken;
    }

    private sealed class SignInResponse
    {
        public string IdToken { get; set; } = "";
        public string LocalId { get; set; } = "";
    }
}