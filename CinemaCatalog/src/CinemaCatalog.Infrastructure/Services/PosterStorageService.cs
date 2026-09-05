using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using CinemaCatalog.Application.Interfaces;

namespace CinemaCatalog.Infrastructure.Services;

public class PosterStorageService(IWebHostEnvironment webHostEnvironment) : IFileStorageService
{
    private const string PostersImgFolder = "posters";
    
    private IWebHostEnvironment WebHostEnvironment { get; } =  webHostEnvironment;
    
    public async Task<string> SaveFileAsync(IFormFile formFile, CancellationToken cancellationToken)
    {
        string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(formFile.FileName)}";
        string folderPath = Path.Combine(WebHostEnvironment.WebRootPath, PostersImgFolder);

        Directory.CreateDirectory(folderPath);

        var filePath = Path.Combine(folderPath, fileName);

        await using var stream = new FileStream(filePath, FileMode.Create);
        await formFile.CopyToAsync(stream, cancellationToken);

        return $"/{PostersImgFolder}/{fileName}";
    }

    public async Task<string> UpdateFileAsync(string existingPath, IFormFile? formFile, CancellationToken cancellationToken)
    {
        if (formFile is not null && formFile.Length > 0)
        {
            DeleteFile(existingPath);
            return await SaveFileAsync(formFile, cancellationToken);
        }

        return existingPath;
    }

    public void DeleteFile(string? existingPath)
    {
        if (string.IsNullOrWhiteSpace(existingPath)) return;

        string filePath = Path.Combine(WebHostEnvironment.WebRootPath, existingPath.TrimStart('/'));

        if (File.Exists(filePath)) File.Delete(filePath);
    }
}