using CinemaCatalog.Domain.Entities;
using CinemaCatalog.Domain.Interfaces;
using CinemaCatalog.Domain.ValueObjects;
using Google.Cloud.Firestore;

namespace CinemaCatalog.Infrastructure.Repositories;

public class FirestoreMovieRepository : IMovieRepository
{
    private readonly FirestoreDb _db;
    private const string Collection = "movies";

    public FirestoreMovieRepository(FirestoreDb db) => _db = db;

    public async Task<List<Movie>> GetPagedMoviesAsync(int page, int pageSize, CancellationToken ct)
    {
        var query = _db.Collection(Collection)
            .OrderBy("Title")
            .Offset((page - 1) * pageSize)
            .Limit(pageSize);

        var snapshot = await query.GetSnapshotAsync(ct);
        return snapshot.Documents.Select(Map).ToList();
    }

    public async Task<int> CountAsync(CancellationToken ct)
    {
        var snapshot = await _db.Collection(Collection).Count().GetSnapshotAsync(ct);
        return (int)(snapshot.Count ?? 0);
    }

    public async Task<Movie?> GetByIdAsync(string id, CancellationToken ct)
    {
        var doc = await _db.Collection(Collection).Document(id).GetSnapshotAsync(ct);
        return doc.Exists ? Map(doc) : null;
    }

    public async Task<Movie?> GetByTitleAsync(string title, CancellationToken ct)
    {
        var snapshot = await _db.Collection(Collection)
            .WhereEqualTo("TitleLower", title.ToLowerInvariant())
            .Limit(1)
            .GetSnapshotAsync(ct);

        return snapshot.Documents.FirstOrDefault() is { } doc ? Map(doc) : null;
    }

    public Task<Movie?> GetByTitleAsNoTrackingAsync(string title, CancellationToken ct)
        => GetByTitleAsync(title, ct);

    public async Task AddAsync(Movie movie, CancellationToken ct)
    {
        var docRef = string.IsNullOrEmpty(movie.Id)
            ? _db.Collection(Collection).Document()
            : _db.Collection(Collection).Document(movie.Id);

        movie.Id = docRef.Id;
        await docRef.SetAsync(ToDict(movie), cancellationToken: ct);
    }

    public async Task RemoveAsync(string id, CancellationToken ct)
        => await _db.Collection(Collection).Document(id).DeleteAsync(cancellationToken: ct);

    public async Task UpdateAsync(Movie movie, CancellationToken ct)
        => await _db.Collection(Collection).Document(movie.Id).SetAsync(ToDict(movie), cancellationToken: ct);

    public async Task<bool> ExistsByTitleAsync(Movie movie, CancellationToken ct)
        => await GetByTitleAsync(movie.Title, ct) is not null;

    private static Movie Map(DocumentSnapshot doc)
    {
        var d = doc.ToDictionary();
        return new Movie
        {
            Id = doc.Id,
            Title = d.GetValueOrDefault("Title")?.ToString() ?? "",
            Plot = d.GetValueOrDefault("Plot")?.ToString() ?? "",
            Director = d.GetValueOrDefault("Director")?.ToString() ?? "",
            Genre = d.GetValueOrDefault("Genre")?.ToString() ?? "",
            ReleaseYear = Year.Parse((d.GetValueOrDefault("ReleaseYear") ?? 0).ToString()),
            Poster = d.GetValueOrDefault("Poster")?.ToString()
        };
    }

    private static Dictionary<string, object> ToDict(Movie m) => new()
    {
        ["Title"] = m.Title,
        ["TitleLower"] = m.Title.ToLowerInvariant(),
        ["Plot"] = m.Plot,
        ["Director"] = m.Director,
        ["Genre"] = m.Genre,
        ["ReleaseYear"] = m.ReleaseYear.ToString(),
        ["Poster"] = m.Poster ?? (object)FieldValue.Delete
    };
}