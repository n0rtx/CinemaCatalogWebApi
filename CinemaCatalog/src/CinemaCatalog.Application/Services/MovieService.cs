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

        if (movie is null)
        {
            movie = await MovieProvider.GetByTitleAsync(title, cancellationToken);

            if (movie is null) return null;

            await MovieRepository.AddAsync(movie, cancellationToken);
            await MovieRepository.SaveChangesAsync(cancellationToken);
        }

        return movie;
    }

    public async Task<Movie?> GetByIdAsync(int id, CancellationToken cancellationToken)
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
        await MovieRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<Movie> UpdateAsync(int id, Movie movie, IFormFile? posterFile,
        CancellationToken cancellationToken)
    {
        var existingMovie = await MovieRepository.GetByIdAsync(id, cancellationToken);

        if (existingMovie is null) throw new KeyNotFoundException("Movie not found");

        if (await IsDuplicatesExistingMovieAfterUpdateAsync(existingMovie, movie, cancellationToken))
            throw new InvalidOperationException("Movie already exists");

        existingMovie.Title = movie.Title;
        existingMovie.Plot = movie.Plot;
        existingMovie.Director = movie.Director;
        existingMovie.Genre = movie.Genre;
        existingMovie.ReleaseYear = movie.ReleaseYear;
        existingMovie.Poster =
            await FileStorageService.UpdateFileAsync(existingMovie.Poster, posterFile, cancellationToken);

        await MovieRepository.SaveChangesAsync(cancellationToken);

        return existingMovie;
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        var movie = await MovieRepository.GetByIdAsync(id, cancellationToken);

        if (movie is null) throw new KeyNotFoundException("Movie not found");

        FileStorageService.DeleteFile(movie.Poster);

        MovieRepository.Remove(movie);
        await MovieRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> IsTitleExistsAsync(Movie movie, CancellationToken cancellationToken)
    {
        return await MovieRepository.ExistsByTitleAsync(movie, cancellationToken);
    }

    private async Task<bool> IsDuplicatesExistingMovieAfterUpdateAsync(Movie existingMovie, Movie newMovie,
        CancellationToken cancellationToken)
    {
        string newTitle = newMovie.Title;

        if (existingMovie.Title.ToLower() == newTitle.ToLower()) return false;

        return await MovieRepository.GetByTitleAsNoTrackingAsync(newTitle, cancellationToken) is not null;
    }
}