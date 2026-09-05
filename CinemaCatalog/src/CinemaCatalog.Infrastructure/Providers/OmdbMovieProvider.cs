using System.Net.Http.Json;
using CinemaCatalog.Domain.Entities;
using CinemaCatalog.Domain.Interfaces;
using CinemaCatalog.Domain.ValueObjects;

namespace CinemaCatalog.Infrastructure.Providers;

public class OmdbMovieProvider : IMovieProvider
{
    private const string BaseUrl = "https://www.omdbapi.com/";

    private readonly HttpClient _httpClient;
    private readonly string _apikey;

    public OmdbMovieProvider(HttpClient httpClient, string apikey)
    {
        _httpClient = httpClient;
        _apikey = apikey;
    }

    public async Task<Movie?> GetByTitleAsync(string title, CancellationToken cancellationToken)
    {
        string requestUrl = $"{BaseUrl}?apikey={_apikey}&t={title}";
        var response = await _httpClient.GetFromJsonAsync<OmdbMovieResponse>(requestUrl, cancellationToken);

        if (response is null || !response.IsSuccess) return null;

        return new Movie
        {
            Title = response.Title ?? "Unknown",
            Plot = response.Plot ?? "Unknown",
            Director = response.Director ?? "Unknown",
            Genre = response.Genre ?? "Unknown",
            ReleaseYear = Year.Parse(response.Year ?? "0"),
            Poster = response.Poster ?? "Unknown",
        };
    }
}