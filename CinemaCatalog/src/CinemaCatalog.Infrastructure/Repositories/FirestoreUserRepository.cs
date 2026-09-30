using CinemaCatalog.Domain.Entities;
using CinemaCatalog.Domain.Interfaces;
using Google.Cloud.Firestore;

namespace CinemaCatalog.Infrastructure.Repositories;

public class FirestoreUserRepository : IUserRepository
{
    private readonly FirestoreDb _db;
    private const string Collection = "users";

    public FirestoreUserRepository(FirestoreDb db) => _db = db;

    public async Task<User?> GetByIdAsync(string id, CancellationToken ct)
    {
        var doc = await _db.Collection(Collection).Document(id).GetSnapshotAsync(ct);
        return doc.Exists ? Map(doc) : null;
    }

    public async Task<User?> GetByLoginAsync(string login, CancellationToken ct)
    {
        var snapshot = await _db.Collection(Collection)
            .WhereEqualTo("LoginLower", login.ToLowerInvariant())
            .Limit(1)
            .GetSnapshotAsync(ct);

        return snapshot.Documents.FirstOrDefault() is { } doc ? Map(doc) : null;
    }

    public async Task AddAsync(User user, CancellationToken ct)
    {
        var docRef = _db.Collection(Collection).Document(user.Id);
        await docRef.SetAsync(new Dictionary<string, object>
        {
            ["Login"] = user.Login,
            ["LoginLower"] = user.Login.ToLowerInvariant(),
            ["CreatedAt"] = Timestamp.FromDateTime(user.CreatedAt.ToUniversalTime())
        }, cancellationToken: ct);
    }

    public async Task DeleteAsync(string id, CancellationToken ct)
        => await _db.Collection(Collection).Document(id).DeleteAsync(cancellationToken: ct);

    private static User Map(DocumentSnapshot doc)
    {
        var d = doc.ToDictionary();
        return new User
        {
            Id = doc.Id,
            Login = d.GetValueOrDefault("Login")?.ToString() ?? "",
            CreatedAt = d.GetValueOrDefault("CreatedAt") is Timestamp ts
                ? ts.ToDateTime()
                : DateTime.UtcNow
        };
    }
}