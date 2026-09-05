using System.Text.Json.Serialization;

namespace CinemaCatalog.Infrastructure.Providers;

public class OmdbMovieResponse
{
    [JsonPropertyName("Title")] public string? Title { get; set; }

    [JsonPropertyName("Year")] public string? Year { get; set; }

    [JsonPropertyName("Genre")] public string? Genre { get; set; }

    [JsonPropertyName("Director")] public string? Director { get; set; }

    [JsonPropertyName("Plot")] public string? Plot { get; set; }

    [JsonPropertyName("Poster")] public string? Poster { get; set; }

    [JsonPropertyName("Response")] public string? Response { get; set; }

    public bool IsSuccess => string.Equals(Response, "True", StringComparison.OrdinalIgnoreCase);
}