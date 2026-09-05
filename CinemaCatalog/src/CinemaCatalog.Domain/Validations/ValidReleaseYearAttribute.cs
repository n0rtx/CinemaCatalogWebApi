using System.ComponentModel.DataAnnotations;
using CinemaCatalog.Domain.ValueObjects;

namespace CinemaCatalog.Domain.Validations;

public class ValidReleaseYearAttribute : ValidationAttribute
{
    public const int MinYear = 1888;
    
    public static int MaxYear => DateTime.UtcNow.Year + 1;

    public ValidReleaseYearAttribute()
    {
        ErrorMessage = $"Release year must be between {MinYear} and {MaxYear}.";
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not Year year) return ValidationResult.Success;
        
        if(!IsYearInRange(year.Start)) return new ValidationResult(ErrorMessage);

        if (year.End.HasValue)
        {
            if(!IsYearInRange(year.End.Value)) return new ValidationResult(ErrorMessage);
            
            if(year.End.Value < year.Start) return new ValidationResult("End year cannot be earlier than start year.");
        }
        
        return ValidationResult.Success;
    }
    
    private static bool IsYearInRange(int year) => year >= MinYear && year <= MaxYear;
}