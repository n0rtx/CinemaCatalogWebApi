using CinemaCatalog.Domain.Entities;

namespace CinemaCatalog.Domain.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<User?> GetByLoginAsync(string login, CancellationToken cancellationToken);
    Task AddAsync(User user, CancellationToken cancellationToken);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
}