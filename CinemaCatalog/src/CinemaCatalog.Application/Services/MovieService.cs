using CinemaCatalog.Application.Interfaces;
using CinemaCatalog.Domain.Entities;
using CinemaCatalog.Domain.Interfaces;
using Microsoft.AspNetCore.Http;

namespace CinemaCatalog.Application.Services;

public class MovieService(
    IMovieRepository movieRepository,
    IMovieProvider movieProvider,
    IFileStorageService fileStorageService) : IMovieService
{
    private IMovieRepository MovieRepository { get; } = movieRepository;
    private IMovieProvider MovieProvider { get; } = movieProvider;
    private IFileStorageService FileStorageService { get; } = fileStorageService;

    public async Task<Movie?> FindOrFetchByTitleAsync(string title, CancellationToken cancellationToken)
    {
        var movie = await MovieRepository.GetByTitleAsync(title, cancellationToken);

        if (movie is not null)
            return movie;

        movie = await MovieProvider.GetByTitleAsync(title, cancellationToken);
        if (movie is null)
            return null;

        await MovieRepository.AddAsync(movie, cancellationToken);
        return movie;
    }

    public async Task<Movie?> GetByIdAsync(string id, CancellationToken cancellationToken)
        => await MovieRepository.GetByIdAsync(id, cancellationToken);

    public async Task CreateAsync(Movie movie, IFormFile? posterFile, CancellationToken cancellationToken)
    {
        if (await IsTitleExistsAsync(movie, cancellationToken))
            throw new InvalidOperationException("Movie already exists");

        if (posterFile is not null && posterFile.Length > 0)
        {
            movie.Poster = await FileStorageService.SaveFileAsync(posterFile, cancellationToken);
        }

        await MovieRepository.AddAsync(movie, cancellationToken);
    }

    public async Task<Movie> UpdateAsync(
        string id,
        Movie movie,
        IFormFile? posterFile,
        CancellationToken cancellationToken)
    {
        var existingMovie = await MovieRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Movie not found");

        if (await IsDuplicateTitleAfterUpdateAsync(existingMovie, movie, cancellationToken))
            throw new InvalidOperationException("Movie already exists");

        existingMovie.Title = movie.Title;
        existingMovie.Plot = movie.Plot;
        existingMovie.Director = movie.Director;
        existingMovie.Genre = movie.Genre;
        existingMovie.ReleaseYear = movie.ReleaseYear;

        if (posterFile is not null && posterFile.Length > 0)
        {
            existingMovie.Poster = await FileStorageService.UpdateFileAsync(
                existingMovie.Poster,
                posterFile,
                cancellationToken);
        }
        else if (!string.IsNullOrWhiteSpace(movie.Poster))
        {
            var url = movie.Poster.Trim();
            if (!string.Equals(existingMovie.Poster, url, StringComparison.Ordinal))
            {
                FileStorageService.DeleteFile(existingMovie.Poster);
                existingMovie.Poster = url;
            }
        }

        await MovieRepository.UpdateAsync(existingMovie, cancellationToken);
        return existingMovie;
    }

    public async Task DeleteAsync(string id, CancellationToken cancellationToken)
    {
        var movie = await MovieRepository.GetByIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException("Movie not found");

        FileStorageService.DeleteFile(movie.Poster);
        await MovieRepository.RemoveAsync(id, cancellationToken);
    }

    public async Task<bool> IsTitleExistsAsync(Movie movie, CancellationToken cancellationToken)
        => await MovieRepository.ExistsByTitleAsync(movie, cancellationToken);

    private async Task<bool> IsDuplicateTitleAfterUpdateAsync(
        Movie existingMovie,
        Movie newMovie,
        CancellationToken cancellationToken)
    {
        if (string.Equals(existingMovie.Title, newMovie.Title, StringComparison.OrdinalIgnoreCase))
            return false;

        return await MovieRepository.GetByTitleAsync(newMovie.Title, cancellationToken) is not null;
    }
}