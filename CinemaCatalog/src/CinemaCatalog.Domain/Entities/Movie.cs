using System.ComponentModel.DataAnnotations;
using CinemaCatalog.Domain.Validations;
using CinemaCatalog.Domain.ValueObjects;

namespace CinemaCatalog.Domain.Entities;

public class Movie
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Please enter a movie title")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 100 characters")]
    public required string Title { get; set; }

    [Required(ErrorMessage = "Please enter the plot.")]
    [StringLength(200, MinimumLength = 10, ErrorMessage = "Plot must be between 10 and 200 characters.")]
    public required string Plot { get; set; }

    [Required(ErrorMessage = "Please enter the director's name.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Director name cannot exceed 100 characters.")]
    public required string Director { get; set; }

    [Required(ErrorMessage = "Please enter the genre.")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Genre cannot exceed 100 characters.")]
    public required string Genre { get; set; }

    [Required(ErrorMessage = "Please enter the release year.")]
    [ValidReleaseYear]
    public required Year ReleaseYear { get; set; }
    
    [StringLength(350)]
    public string? Poster { get; set; }
}