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
            Poster = string.IsNullOrEmpty(movie.Poster) || baseUrl is null
                ? movie.Poster
                : $"{baseUrl}{movie.Poster}"
        };
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

    public static Movie ToEntity(this UpdateMovieDto dto, int id)
    {
        return new Movie
        {
            Id = id,
            Title = dto.Title,
            Plot = dto.Plot,
            Director = dto.Director,
            Genre = dto.Genre,
            ReleaseYear = dto.ReleaseYear
        };
    }
}