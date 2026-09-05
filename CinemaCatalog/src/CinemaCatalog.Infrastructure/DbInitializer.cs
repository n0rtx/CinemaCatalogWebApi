using CinemaCatalog.Domain.Entities;
using CinemaCatalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CinemaCatalog.Infrastructure;

public static class DbInitializer
{
    public static async Task InitializeDbAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await SeedDataAsync(context);
    }

    private static async Task SeedDataAsync(ApplicationDbContext context)
    {
        if (await context.Movies.AnyAsync()) return;

        context.Movies.AddRange(
            new Movie
            {
                Title = "Game of Thrones",
                Plot =
                    "Nine noble families fight for control over the lands of Westeros, while an ancient enemy returns after being dormant for millennia.",
                Director = "N/A", Genre = "Action, Adventure, Drama",
                ReleaseYear = new Year { Start = 2011, End = 2019 },
                Poster =
                    "https://m.media-amazon.com/images/M/MV5BMTNhMDJmNmYtNDQ5OS00ODdlLWE0ZDAtZTgyYTIwNDY3OTU3XkEyXkFqcGc@._V1_QL75_UX380_CR0,3,380,562_.jpg"
            },
            new Movie
            {
                Title = "Breaking Bad",
                Plot =
                    "A high school chemistry teacher turns to cooking methamphetamine after a cancer diagnosis, gradually transforming into a ruthless criminal mastermind.",
                Director = "N/A", Genre = "Crime, Drama, Thriller", ReleaseYear = new Year { Start = 2008, End = 2013 },
                Poster =
                    "https://m.media-amazon.com/images/M/MV5BMzU5ZGYzNmQtMTdhYy00OGRiLTg0NmQtYjVjNzliZTg1ZGE4XkEyXkFqcGc@._V1_QL75_UX380_CR0,4,380,562_.jpg"
            },
            new Movie
            {
                Title = "Inception",
                Plot =
                    "A skilled thief who steals secrets through dream-sharing technology is offered a chance to have his criminal record erased in exchange for planting an idea in a target's mind.",
                Director = "Christopher Nolan", Genre = "Action, Adventure, Sci-Fi",
                ReleaseYear = new Year { Start = 2010, End = null },
                Poster =
                    "https://m.media-amazon.com/images/M/MV5BMjAxMzY3NjcxNF5BMl5BanBnXkFtZTcwNTI5OTM0Mw@@._V1_QL75_UX380_CR0,0,380,562_.jpg"
            },
            new Movie
            {
                Title = "Stranger Things",
                Plot =
                    "When a young boy disappears, his mother, a police chief, and his friends must confront terrifying supernatural forces in order to get him back.",
                Director = "N/A", Genre = "Drama, Fantasy, Horror", ReleaseYear = new Year { Start = 2016, End = null },
                Poster =
                    "https://m.media-amazon.com/images/M/MV5BNjRiMTA4NWUtNmE0ZC00NGM0LWJhMDUtZWIzMDM5ZDIzNTg3XkEyXkFqcGc@._V1_QL75_UY562_CR35,0,380,562_.jpg"
            },
            new Movie
            {
                Title = "The Dark Knight",
                Plot =
                    "When a criminal mastermind known as the Joker wreaks havoc on Gotham, Batman must confront one of the greatest psychological and physical tests of his ability to fight injustice.",
                Director = "Christopher Nolan", Genre = "Action, Crime, Drama",
                ReleaseYear = new Year { Start = 2008, End = null },
                Poster =
                    "https://m.media-amazon.com/images/M/MV5BMTMxNTMwODM0NF5BMl5BanBnXkFtZTcwODAyMTk2Mw@@._V1_QL75_UX380_CR0,0,380,562_.jpg"
            },
            new Movie
            {
                Title = "The Office",
                Plot =
                    "A mockumentary crew follows the daily lives of employees at the Scranton, Pennsylvania branch of a paper supply company.",
                Director = "N/A", Genre = "Comedy", ReleaseYear = new Year { Start = 2005, End = 2013 },
                Poster =
                    "https://m.media-amazon.com/images/M/MV5BZjQwYzBlYzUtZjhhOS00ZDQ0LWE0NzAtYTk4MjgzZTNkZWEzXkEyXkFqcGc@._V1_QL75_UX380_CR0,4,380,562_.jpg"
            },
            new Movie
            {
                Title = "Interstellar",
                Plot =
                    "A team of explorers travels through a wormhole in space in an attempt to ensure humanity's survival as Earth becomes uninhabitable.",
                Director = "Christopher Nolan", Genre = "Adventure, Drama, Sci-Fi",
                ReleaseYear = new Year { Start = 2014, End = null },
                Poster =
                    "https://m.media-amazon.com/images/M/MV5BYzdjMDAxZGItMjI2My00ODA1LTlkNzItOWFjMDU5ZDJlYWY3XkEyXkFqcGc@._V1_QL75_UX380_CR0,0,380,562_.jpg"
            },
            new Movie
            {
                Title = "Friends",
                Plot =
                    "Follows the personal and professional lives of six friends living in Manhattan as they navigate work, romance, and everyday life.",
                Director = "N/A", Genre = "Comedy, Romance", ReleaseYear = new Year { Start = 1994, End = 2004 },
                Poster =
                    "https://m.media-amazon.com/images/M/MV5BOTU2YmM5ZjctOGVlMC00YTczLTljM2MtYjhlNGI5YWMyZjFkXkEyXkFqcGc@._V1_QL75_UY562_CR1,0,380,562_.jpg"
            },
            new Movie
            {
                Title = "The Matrix",
                Plot =
                    "A computer programmer discovers that the reality he knows is a simulation controlled by machines, and joins a rebellion to fight back.",
                Director = "Lana Wachowski, Lilly Wachowski", Genre = "Action, Sci-Fi",
                ReleaseYear = new Year { Start = 1999, End = null },
                Poster =
                    "https://m.media-amazon.com/images/M/MV5BN2NmN2VhMTQtMDNiOS00NDlhLTliMjgtODE2ZTY0ODQyNDRhXkEyXkFqcGc@._V1_QL75_UX380_CR0,4,380,562_.jpg"
            },
            new Movie
            {
                Title = "The Mandalorian",
                Plot =
                    "A lone bounty hunter travels the outer reaches of the galaxy, far from the authority of the New Republic, taking on dangerous jobs while protecting a mysterious child.",
                Director = "N/A", Genre = "Action, Adventure, Fantasy",
                ReleaseYear = new Year { Start = 2019, End = null },
                Poster =
                    "https://m.media-amazon.com/images/M/MV5BNjgxZGM0OWUtZGY1MS00MWRmLTk2N2ItYjQyZTI1OThlZDliXkEyXkFqcGc@._V1_SX300.jpg"
            });

        await context.SaveChangesAsync();
    }
}