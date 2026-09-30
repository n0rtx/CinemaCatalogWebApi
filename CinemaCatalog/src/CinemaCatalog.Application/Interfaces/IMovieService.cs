using CinemaCatalog.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CinemaCatalog.Application.Interfaces;

public interface IMovieService
{
    Task<Movie?> FindOrFetchByTitleAsync(string title, CancellationToken cancellationToken);

    Task<Movie?> GetByIdAsync(string id, CancellationToken cancellationToken);

    Task CreateAsync(Movie movie, IFormFile? formFile, CancellationToken cancellationToken);

    Task<Movie> UpdateAsync(string id, Movie movie, IFormFile? posterFile, CancellationToken cancellationToken);

    Task DeleteAsync(string id, CancellationToken cancellationToken);

    Task<bool> IsTitleExistsAsync(Movie movie, CancellationToken cancellationToken);
}