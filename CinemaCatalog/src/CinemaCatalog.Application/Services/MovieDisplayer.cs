using CinemaCatalog.Application.Interfaces;
using CinemaCatalog.Domain.Entities;
using CinemaCatalog.Domain.Interfaces;

namespace CinemaCatalog.Application.Services;

public class MovieDisplayer(IMovieRepository movieRepository) : IEntityDisplayer<Movie>
{
    private IMovieRepository MovieRepository { get; } = movieRepository;

    private const int PageSize = 9;

    public async Task<int> CountEntitiesAsync(CancellationToken cancellationToken) =>
        await MovieRepository.CountAsync(cancellationToken);

    public async Task<int> CountPagesAsync(CancellationToken cancellationToken)
    {
        int amountOfEntities = await CountEntitiesAsync(cancellationToken);

        return (int)Math.Ceiling(amountOfEntities / (double)PageSize);
    }


    public async Task<List<Movie>> GetPagedEntitiesAsync(int page, CancellationToken cancellationToken)
        => await MovieRepository.GetPagedMoviesAsync(page, PageSize, cancellationToken);
}