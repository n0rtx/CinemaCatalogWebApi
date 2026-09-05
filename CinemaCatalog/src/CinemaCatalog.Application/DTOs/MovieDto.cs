using CinemaCatalog.Domain.ValueObjects;

namespace CinemaCatalog.Application.DTOs;

public class MovieDto
{
    public int Id { get; set; }
    
    public required string Title { get; set; }
    
    public required string Plot { get; set; }
    
    public required string Director { get; set; }
    
    public required string Genre { get; set; }
    
    public required Year ReleaseYear { get; set; }
    
    public string? Poster { get; set; }
}