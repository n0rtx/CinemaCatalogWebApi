namespace CinemaCatalog.Application.Interfaces;

public interface IEntityDisplayer<T>
{
    Task<int> CountEntitiesAsync(CancellationToken cancellationToken);

    Task<int> CountPagesAsync(CancellationToken cancellationToken);

    Task<List<T>> GetPagedEntitiesAsync(int page, CancellationToken cancellationToken);
}