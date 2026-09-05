using CinemaCatalog.Domain.Entities;

namespace CinemaCatalog.Domain.Interfaces;

public interface IMovieProvider
{
    Task<Movie?> GetByTitleAsync(string title, CancellationToken cancellationToken);
}