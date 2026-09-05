using CinemaCatalog.Domain.Entities;
using CinemaCatalog.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaCatalog.Infrastructure.EntityTypeConfigurations;

public class MovieEntityTypeConfiguration : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder.HasKey(m => m.Id);
        builder.Property(m => m.Id).ValueGeneratedOnAdd();

        builder.Property(m => m.Title)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.Plot)
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(m => m.Director)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.Genre)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(m => m.ReleaseYear)
            .HasConversion(
                year => year.ToString(),
                raw => Year.Parse(raw))
            .HasMaxLength(20)
            .IsRequired();

        builder.Property(m => m.Poster)
            .HasMaxLength(350);

        builder.HasIndex(m => m.Title)
            .IsUnique();

        builder.ToTable(m => m.HasCheckConstraint(
            name: "CK__Movie__Title",
            sql: $"LEN({nameof(Movie.Title)}) > 0"));
        
        builder.ToTable(m => m.HasCheckConstraint(
            name: "CK__Movie__Plot",
            sql: $"LEN({nameof(Movie.Plot)}) > 0"));
        
        builder.ToTable(m => m.HasCheckConstraint(
            name: "CK__Movie__Director",
            sql: $"LEN({nameof(Movie.Director)}) > 0"));
        
        builder.ToTable(m => m.HasCheckConstraint(
            name: "CK__Movie__Genre",
            sql: $"LEN({nameof(Movie.Genre)}) > 0"));
        
        builder.ToTable(m => m.HasCheckConstraint(
            name: "CK__Movie__ReleaseYear",
            sql: $"LEN({nameof(Movie.ReleaseYear)}) > 0"));
    }
}