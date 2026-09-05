using CinemaCatalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaCatalog.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<Movie> Movies { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}