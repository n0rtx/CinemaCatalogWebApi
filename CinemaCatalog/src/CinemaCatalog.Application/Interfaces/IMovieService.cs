using CinemaCatalog.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace CinemaCatalog.Application.Interfaces;

public interface IMovieService
{
    Task<Movie?> FindOrFetchByTitleAsync(string title, CancellationToken cancellationToken);

    Task<Movie?> GetByIdAsync(int id, CancellationToken cancellationToken);

    Task CreateAsync(Movie movie, IFormFile? formFile, CancellationToken cancellationToken);

    Task<Movie> UpdateAsync(int id, Movie movie, IFormFile? posterFile,CancellationToken cancellationToken);

    Task DeleteAsync(int id, CancellationToken cancellationToken);

    Task<bool> IsTitleExistsAsync(Movie movie, CancellationToken cancellationToken);
}