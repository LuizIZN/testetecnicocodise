using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using TesteTecnico.Application.Interfaces;

namespace TesteTecnico.Infraestructure.Storage;

public sealed class LocalImageStorage(IWebHostEnvironment environment) : IImageStorage
{
    private const long MaxFileSize = 5 * 1024 * 1024;
    private static readonly Dictionary<string, string> AllowedContentTypes = new(StringComparer.OrdinalIgnoreCase)
    {
        [".jpg"] = "image/jpeg",
        [".jpeg"] = "image/jpeg",
        [".png"] = "image/png",
        [".webp"] = "image/webp"
    };

    private readonly IWebHostEnvironment _environment = environment;

    public async Task<string> SaveAsync(
        IFormFile image,
        string folder,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(image);

        if (image.Length == 0)
        {
            throw new InvalidOperationException("A imagem não pode estar vazia.");
        }

        if (image.Length > MaxFileSize)
        {
            throw new InvalidOperationException("A imagem não pode exceder 5 MB.");
        }

        var extension = Path.GetExtension(image.FileName);
        if (!AllowedContentTypes.TryGetValue(extension, out var contentType) ||
            !string.Equals(image.ContentType, contentType, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Formato de imagem inválido. Use JPG, PNG ou WEBP.");
        }

        var safeFolder = folder.Trim('/', '\\');
        var webRootPath = _environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        }

        var directory = Path.Combine(webRootPath, safeFolder);
        Directory.CreateDirectory(directory);

        var fileName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var filePath = Path.Combine(directory, fileName);

        await using var stream = File.Create(filePath);
        await image.CopyToAsync(stream, cancellationToken);

        return $"/{safeFolder.Replace('\\', '/')}/{fileName}";
    }

    public Task DeleteAsync(
        string? imageUrl,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(imageUrl))
        {
            return Task.CompletedTask;
        }

        var relativePath = imageUrl.TrimStart('/', '\\')
            .Replace('/', Path.DirectorySeparatorChar)
            .Replace('\\', Path.DirectorySeparatorChar);
        var webRootPath = _environment.WebRootPath;
        if (string.IsNullOrWhiteSpace(webRootPath))
        {
            webRootPath = Path.Combine(_environment.ContentRootPath, "wwwroot");
        }

        var root = Path.GetFullPath(webRootPath);
        var fullPath = Path.GetFullPath(Path.Combine(root, relativePath));
        if (!fullPath.StartsWith(root + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("O caminho da imagem é inválido.");
        }

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return Task.CompletedTask;
    }

    public Task<string> UpdateAsync(
        string? oldImageUrl,
        IFormFile newImage,
        string folder,
        CancellationToken cancellationToken = default)
    {
        return DeleteAsync(oldImageUrl, cancellationToken)
            .ContinueWith(_ => SaveAsync(newImage, folder, cancellationToken), cancellationToken)
            .Unwrap();
    }
}
