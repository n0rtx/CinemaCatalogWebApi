namespace CinemaCatalog.Application.DTOs;

public class PagedMovieResponse
{
    public int Page { get; set; }

    public int TotalPages { get; set; }

    public IEnumerable<MovieDto> Movies { get; set; } = [];
}