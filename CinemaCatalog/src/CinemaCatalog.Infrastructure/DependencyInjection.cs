using CinemaCatalog.Application.Interfaces;
using CinemaCatalog.Application.Interfaces.Auth;
using CinemaCatalog.Application.Services;
using CinemaCatalog.Domain.Entities;
using CinemaCatalog.Domain.Interfaces;
using CinemaCatalog.Infrastructure.Firebase;
using CinemaCatalog.Infrastructure.Providers;
using CinemaCatalog.Infrastructure.Repositories;
using CinemaCatalog.Infrastructure.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaCatalog.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var firebaseSection = configuration.GetSection(FirebaseSettings.SectionName);
        var projectId = firebaseSection["ProjectId"]
                        ?? throw new InvalidOperationException("Firebase:ProjectId is missing");
        var credentialsPath = firebaseSection["CredentialsPath"]
                              ?? throw new InvalidOperationException("Firebase:CredentialsPath is missing");

        if (!Path.IsPathRooted(credentialsPath))
        {
            credentialsPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, credentialsPath));
            if (!File.Exists(credentialsPath))
            {
                var contentRoot = configuration["ContentRoot"]
                                  ?? Directory.GetCurrentDirectory();
                credentialsPath = Path.GetFullPath(Path.Combine(contentRoot, firebaseSection["CredentialsPath"]!));
            }
        }

        if (!File.Exists(credentialsPath))
            throw new FileNotFoundException(
                $"Firebase credentials file not found: {credentialsPath}");

        var credential = GoogleCredential.FromFile(credentialsPath);

        if (FirebaseApp.DefaultInstance is null)
        {
            FirebaseApp.Create(new AppOptions
            {
                Credential = credential,
                ProjectId = projectId
            });
        }

        services.AddSingleton(_ => new FirestoreDbBuilder
        {
            ProjectId = projectId,
            Credential = credential
        }.Build());

        services.AddScoped<IMovieRepository, FirestoreMovieRepository>();
        services.AddScoped<IUserRepository, FirestoreUserRepository>();
        services.AddScoped<IMovieService, MovieService>();
        services.AddScoped<IAuthService, FirebaseAuthService>();
        services.AddScoped<IFileStorageService, PosterStorageService>();
        services.AddScoped<IEntityDisplayer<Movie>, MovieDisplayer>();

        string omdbApiKey = configuration["Omdb:ApiKey"]
                            ?? throw new InvalidOperationException("No Omdb ApiKey was found");
        string omdbUrl = configuration["Omdb:BaseUrl"]
                         ?? throw new InvalidOperationException("No Omdb Url was found");

        services.AddHttpClient(nameof(OmdbMovieProvider), client => client.BaseAddress = new Uri(omdbUrl));
        services.AddScoped<IMovieProvider>(sp =>
        {
            var factory = sp.GetRequiredService<IHttpClientFactory>();
            var client = factory.CreateClient(nameof(OmdbMovieProvider));
            return new OmdbMovieProvider(client, omdbApiKey);
        });

        return services;
    }
}