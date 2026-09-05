using Microsoft.AspNetCore.Http;

namespace CinemaCatalog.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile? formFile, CancellationToken cancellationToken);
    
    Task<string> UpdateFileAsync(string existingPath, IFormFile? formFile, CancellationToken cancellationToken);
    
    void DeleteFile(string? existingPath);
}