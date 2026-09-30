using CinemaCatalog.Domain.Entities;

namespace CinemaCatalog.Domain.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetPagedMoviesAsync(int page, int pageSize, CancellationToken cancellationToken);
    Task<int> CountAsync(CancellationToken cancellationToken);
    Task<Movie?> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<Movie?> GetByTitleAsync(string title, CancellationToken cancellationToken);
    Task<Movie?> GetByTitleAsNoTrackingAsync(string title, CancellationToken cancellationToken);
    Task AddAsync(Movie movie, CancellationToken cancellationToken);
    Task RemoveAsync(string id, CancellationToken cancellationToken);
    Task UpdateAsync(Movie movie, CancellationToken cancellationToken);
    Task<bool> ExistsByTitleAsync(Movie movie, CancellationToken cancellationToken);
}