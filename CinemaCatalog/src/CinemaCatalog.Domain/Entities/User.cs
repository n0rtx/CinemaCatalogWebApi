using System.ComponentModel.DataAnnotations;

namespace CinemaCatalog.Domain.Entities;

public class User
{
    public string Id { get; set; } = string.Empty;

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Login { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public string? Password { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}