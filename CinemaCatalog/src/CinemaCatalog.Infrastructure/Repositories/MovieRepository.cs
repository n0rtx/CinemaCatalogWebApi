using CinemaCatalog.Domain.Entities;
using CinemaCatalog.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaCatalog.Infrastructure.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly ApplicationDbContext _dbContext;

    public MovieRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Movie>> GetPagedMoviesAsync(int page, int pageSize, CancellationToken cancellationToken)
    {
        return await _dbContext.Movies.OrderBy(m => m.Title)
            .Skip((page - 1) * pageSize).
            Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public Task<int> CountAsync(CancellationToken cancellationToken)
    {
        return _dbContext.Movies.CountAsync(cancellationToken);
    }

    public async Task<Movie?> GetByIdAsync(int id, CancellationToken cancellationToken)
        => await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);

    public async Task<Movie?> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        title = title.ToLower();
        return await _dbContext.Movies.FirstOrDefaultAsync(m => m.Title.ToLower() == title, cancellationToken);
    }

    public async Task<Movie?> GetByTitleAsNoTrackingAsync(string title, CancellationToken cancellationToken)
    {
        title = title.ToLower();
        
        return await _dbContext.Movies.AsNoTracking().FirstOrDefaultAsync(m => m.Title.ToLower() == title, cancellationToken);
    }

    public async Task AddAsync(Movie movie, CancellationToken cancellationToken)
        => await _dbContext.Movies.AddAsync(movie, cancellationToken);

    public void Remove(Movie movie)
        => _dbContext.Movies.Remove(movie);

    public void Update(Movie movie) => _dbContext.Movies.Update(movie);

    public async Task<bool> ExistsByTitleAsync(Movie movie, CancellationToken cancellationToken)
    {
        string title = movie.Title.ToLower();
    
        return await _dbContext.Movies.AnyAsync(m => m.Title.ToLower() == title, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken)
        => await _dbContext.SaveChangesAsync(cancellationToken);
}