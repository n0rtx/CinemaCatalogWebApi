using CinemaCatalog.Domain.Entities;

namespace CinemaCatalog.Application.DTOs;

public static class MovieMappingExtensions
{
    public static MovieDto ToDto(this Movie movie, string? baseUrl = null)
    {
        return new MovieDto
        {
            Id = movie.Id,
            Title = movie.Title,
            Plot = movie.Plot,
            Director = movie.Director,
            Genre = movie.Genre,
            ReleaseYear = movie.ReleaseYear,
            Poster = ResolvePoster(movie.Poster, baseUrl)
        };
    }

    private static string? ResolvePoster(string? poster, string? baseUrl)
    {
        if (string.IsNullOrWhiteSpace(poster) || poster is "N/A" or "Unknown")
            return null;

        if (poster.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
            || poster.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
            return poster;

        if (string.IsNullOrEmpty(baseUrl))
            return poster;

        return $"{baseUrl.TrimEnd('/')}{(poster.StartsWith('/') ? poster : "/" + poster)}";
    }

    public static Movie ToEntity(this CreateMovieDto dto)
    {
        return new Movie
        {
            Title = dto.Title,
            Plot = dto.Plot,
            Director = dto.Director,
            Genre = dto.Genre,
            ReleaseYear = dto.ReleaseYear
        };
    }

    public static Movie ToEntity(this UpdateMovieDto dto, string id)
    {
        return new Movie
        {
            Id = id,
            Title = dto.Title,
            Plot = dto.Plot,
            Director = dto.Director,
            Genre = dto.Genre,
            ReleaseYear = dto.ReleaseYear,
            Poster = string.IsNullOrWhiteSpace(dto.PosterUrl) ? null : dto.PosterUrl.Trim()
        };
    }
}