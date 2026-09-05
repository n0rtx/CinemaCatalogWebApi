using CinemaCatalog.Application.DTOs;
using CinemaCatalog.Application.Interfaces;
using CinemaCatalog.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CinemaCatalog.WebApi.Controllers;

[ApiController]
[Route("api/movies")]
public class MoviesController(IMovieService movieService, IEntityDisplayer<Movie> entityDisplayer) : ControllerBase
{
    private string BaseUrl => $"{Request.Scheme}://{Request.Host}";

    private IMovieService MovieService { get; } = movieService;

    private IEntityDisplayer<Movie> EntityDisplayer { get; } = entityDisplayer;

    [HttpGet]
    public async Task<ActionResult<PagedMovieResponse>> GetAllAsync(CancellationToken cancellationToken, int page = 1)
    {
        if (page < 1) page = 1;

        int amountOfPages = await EntityDisplayer.CountPagesAsync(cancellationToken);
        var movies = await EntityDisplayer.GetPagedEntitiesAsync(page, cancellationToken);

        return Ok(new PagedMovieResponse()
        {
            Page = page,
            TotalPages = amountOfPages,
            Movies = movies.Select(m => m.ToDto(BaseUrl))
        });
    }

    [HttpGet("search")]
    public async Task<ActionResult<MovieDto>> SearchByTitleAsync([FromQuery] string? title,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(title)) return BadRequest("No title has been provided.");

        var movie = await MovieService.FindOrFetchByTitleAsync(title, cancellationToken);

        if (movie is null) return NotFound($"No movie the title \"{title}\" was found.");

        return Ok(movie.ToDto(BaseUrl));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<MovieDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var movie = await MovieService.GetByIdAsync(id, cancellationToken);

        if (movie is null) return NotFound();

        return Ok(movie.ToDto(BaseUrl));
    }

    [HttpPost]
    public async Task<ActionResult<MovieDto>> CreateAsync([FromForm] CreateMovieDto createMovieDto,
        [FromForm] IFormFile? formFile,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var movie = createMovieDto.ToEntity();

        try
        {
            await MovieService.CreateAsync(movie, formFile, cancellationToken);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }

        return CreatedAtAction(nameof(GetByIdAsync), new { id = movie.Id }, movie.ToDto(BaseUrl));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync(int id, [FromForm] UpdateMovieDto updateMovieDto,
        [FromForm] IFormFile? formFile, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);

        var movie = updateMovieDto.ToEntity(id);

        try
        {
            await MovieService.UpdateAsync(id, movie, formFile, cancellationToken);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync(int id, CancellationToken cancellationToken)
    {
        try
        {
            await MovieService.DeleteAsync(id, cancellationToken);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        
        return NoContent();
    }
}