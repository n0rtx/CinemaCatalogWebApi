using CinemaCatalog.Domain.ValueObjects;

namespace CinemaCatalog.Application.DTOs;

public class CreateMovieDto
{
    public required string Title { get; set; }
 
    public required string Plot { get; set; }
 
    public required string Director { get; set; }
 
    public required string Genre { get; set; }
 
    public required Year ReleaseYear { get; set; }
}