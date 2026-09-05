using CinemaCatalog.Domain.Entities;

namespace CinemaCatalog.Domain.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetPagedMoviesAsync(int page, int pageSize, CancellationToken cancellationToken);
    
    Task<int> CountAsync(CancellationToken cancellationToken);
    
    Task<Movie?> GetByIdAsync(int id, CancellationToken cancellationToken);
    
    Task<Movie?> GetByTitleAsync(string title, CancellationToken cancellationToken);
    
    Task<Movie?> GetByTitleAsNoTrackingAsync(string title, CancellationToken cancellationToken);
    
    Task AddAsync(Movie movie, CancellationToken cancellationToken);

    void Remove(Movie movie);

    public void Update(Movie movie);
    
    public Task<bool> ExistsByTitleAsync(Movie movie, CancellationToken cancellationToken);
    
    Task SaveChangesAsync(CancellationToken cancellationToken);
}