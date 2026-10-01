using CinemaCatalog.Application.Interfaces;
using CinemaCatalog.Domain.Entities;
using CinemaCatalog.Domain.Interfaces;
using CinemaCatalog.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace CinemaCatalog.Infrastructure.Seed;

public class FirestoreDataSeeder(
    IMovieRepository movieRepository,
    ILogger<FirestoreDataSeeder> logger) : IDataSeeder
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        var existingCount = await movieRepository.CountAsync(cancellationToken);

        if (existingCount > 0)
        {
            logger.LogInformation("Firestore already contains {Count} movies. Skipping seed.", existingCount);
            return;
        }

        logger.LogInformation("Seeding Firestore with sample movies...");

        var movies = GetSeedMovies();

        foreach (var movie in movies)
        {
            await movieRepository.AddAsync(movie, cancellationToken);
        }

        logger.LogInformation("Successfully seeded {Count} movies.", movies.Count);
    }

    private static List<Movie> GetSeedMovies() =>
    [
        new Movie
        {
            Title = "Inception",
            Plot = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a C.E.O.",
            Director = "Christopher Nolan",
            Genre = "Action, Sci-Fi, Thriller",
            ReleaseYear = Year.Parse("2010"),
            Poster = "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_.jpg"
        },
        new Movie
        {
            Title = "The Shawshank Redemption",
            Plot = "A banker convicted of uxoricide forms a friendship over a quarter century with a hardened convict, while maintaining his innocence and trying to remain hopeful through simple compassion.",
            Director = "Frank Darabont",
            Genre = "Drama",
            ReleaseYear = Year.Parse("1994"),
            Poster = "https://m.media-amazon.com/images/M/MV5BMDAyY2FhYjctNDc5OS00MDNlLThiMGUtY2UxYWVkNGY2ZjljXkEyXkFqcGc@._V1_.jpg"
        },
        new Movie
        {
            Title = "The Dark Knight",
            Plot = "When a menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman, James Gordon and Harvey Dent must work together to put an end to the madness.",
            Director = "Christopher Nolan",
            Genre = "Action, Crime, Drama",
            ReleaseYear = Year.Parse("2008"),
            Poster = "https://m.media-amazon.com/images/S/pv-target-images/8753733ac616155963cc440c3cf5367f45d7685b672c5b9c35bc7f182aec17c4.jpg"
        },
        new Movie
        {
            Title = "Pulp Fiction",
            Plot = "The lives of two mob hitmen, a boxer, a gangster and his wife, and a pair of diner bandits intertwine in four tales of violence and redemption.",
            Director = "Quentin Tarantino",
            Genre = "Crime, Drama",
            ReleaseYear = Year.Parse("1994"),
            Poster = "https://upload.wikimedia.org/wikipedia/en/3/3b/Pulp_Fiction_%281994%29_poster.jpg?utm_source=en.wikipedia.org&utm_campaign=index&utm_content=original"
        },
        new Movie
        {
            Title = "Interstellar",
            Plot = "When Earth becomes uninhabitable in the future, a farmer and ex-NASA pilot, Joseph Cooper, is tasked to pilot a spacecraft, along with a team of researchers, to find a new planet for humans.",
            Director = "Christopher Nolan",
            Genre = "Adventure, Drama, Sci-Fi",
            ReleaseYear = Year.Parse("2014"),
            Poster = "https://m.media-amazon.com/images/M/MV5BYzdjMDAxZGItMjI2My00ODA1LTlkNzItOWFjMDU5ZDJlYWY3XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
        },
        new Movie
        {
            Title = "The Matrix",
            Plot = "When a beautiful stranger leads computer hacker Neo to a forbidding underworld, he discovers the shocking truth--the life he knows is the elaborate deception of an evil cyber-intelligence.",
            Director = "Lana Wachowski, Lilly Wachowski",
            Genre = "Action, Sci-Fi",
            ReleaseYear = Year.Parse("1999"),
            Poster = "https://m.media-amazon.com/images/M/MV5BN2NmN2VhMTQtMDNiOS00NDlhLTliMjgtODE2ZTY0ODQyNDRhXkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
        },
        new Movie
        {
            Title = "Forrest Gump",
            Plot = "The history of the United States from the 1950s to the 1970s unfolds from the perspective of an Alabama man with an IQ of 75, whose only desire is to be reunited with his childhood sweetheart.",
            Director = "Robert Zemeckis",
            Genre = "Drama, Romance",
            ReleaseYear = Year.Parse("1994"),
            Poster = "https://upload.wikimedia.org/wikipedia/en/6/67/Forrest_Gump_poster.jpg?utm_source=en.wikipedia.org&utm_campaign=index&utm_content=original"
        },
        new Movie
        {
            Title = "The Godfather",
            Plot = "The aging patriarch of an organized crime dynasty transfers control of his clandestine empire to his reluctant son.",
            Director = "Francis Ford Coppola",
            Genre = "Crime, Drama",
            ReleaseYear = Year.Parse("1972"),
            Poster = "https://m.media-amazon.com/images/M/MV5BNGEwYjgwOGQtYjg5ZS00Njc1LTk2ZGEtM2QwZWQ2NjdhZTE5XkEyXkFqcGc@._V1_FMjpg_UX1000_.jpg"
        }
    ];
}