using CinemaCatalog.Domain.Entities;

namespace CinemaCatalog.Application.Interfaces.Auth;

public interface IAuthService
{
    Task<(User User, string IdToken)> RegisterAsync(string login, string password, CancellationToken cancellationToken);
    Task<(User User, string IdToken)> LoginAsync(string login, string password, CancellationToken cancellationToken);
}