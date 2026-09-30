using System.Net.Http.Json;
using CinemaCatalog.Application.Interfaces;
using CinemaCatalog.Application.Interfaces.Auth;
using CinemaCatalog.Domain.Entities;
using CinemaCatalog.Domain.Interfaces;
using FirebaseAdmin.Auth;

namespace CinemaCatalog.Infrastructure.Services;

public class FirebaseAuthService : IAuthService
{
    private readonly IUserRepository _userRepository;

    public FirebaseAuthService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
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

    private static async Task<string> SignInWithPasswordAsync(string email, string password, CancellationToken ct)
    {
        var apiKey = Environment.GetEnvironmentVariable("FIREBASE_WEB_API_KEY")
                     ?? throw new InvalidOperationException("FIREBASE_WEB_API_KEY is not set");

        using var http = new HttpClient();
        var url = $"https://identitytoolkit.googleapis.com/v1/accounts:signInWithPassword?key={apiKey}";
        var body = new { email, password, returnSecureToken = true };

        var response = await http.PostAsJsonAsync(url, body, ct);
        response.EnsureSuccessStatusCode();

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