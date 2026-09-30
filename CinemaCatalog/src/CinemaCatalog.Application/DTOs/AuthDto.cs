using System.ComponentModel.DataAnnotations;

namespace CinemaCatalog.Application.DTOs;

public class RegisterRequest
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public required string Login { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 6)]
    public required string Password { get; set; }
}

public class LoginRequest
{
    [Required]
    public required string Login { get; set; }

    [Required]
    public required string Password { get; set; }
}

public class AuthResponse
{
    public required string Id { get; set; }
    public required string Login { get; set; }
    public required string IdToken { get; set; }
}