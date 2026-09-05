using CinemaCatalog.Application.Interfaces;
using CinemaCatalog.Application.Services;
using CinemaCatalog.Domain.Entities;
using CinemaCatalog.Domain.Interfaces;
using CinemaCatalog.Infrastructure.Providers;
using CinemaCatalog.Infrastructure.Repositories;
using CinemaCatalog.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DatabaseConnection") ??
                               throw new InvalidOperationException("No connection string was found");

        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

        services.AddScoped<IMovieRepository, MovieRepository>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IFileStorageService, PosterStorageService>();
        services.AddScoped<IEntityDisplayer<Movie>, MovieDisplayer>();

        string omdbApiKey = configuration["Omdb:ApiKey"] ??
                            throw new InvalidOperationException("No Omdb ApiKey was found");
        string omdbUrl = configuration["Omdb:BaseUrl"] ?? throw new InvalidOperationException("No Omdb Url was found");

        services.AddHttpClient(nameof(OmdbMovieProvider), client => { client.BaseAddress = new Uri(omdbUrl); });
        services.AddScoped<IMovieProvider>(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(nameof(OmdbMovieProvider));
            return new OmdbMovieProvider(httpClient, omdbApiKey);
        });

        return services;
    }
}