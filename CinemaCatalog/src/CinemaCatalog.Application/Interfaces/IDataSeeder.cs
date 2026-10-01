namespace CinemaCatalog.Application.Interfaces;

public interface IDataSeeder
{
    Task SeedAsync(CancellationToken cancellationToken = default);
}