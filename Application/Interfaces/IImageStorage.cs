using Microsoft.AspNetCore.Http;

namespace TesteTecnico.Application.Interfaces;

public interface IImageStorage
{
    Task<string> SaveAsync(
        IFormFile image,
        string folder,
        CancellationToken cancellationToken = default);

    Task DeleteAsync(
        string? imageUrl,
        CancellationToken cancellationToken = default);

    Task<string> UpdateAsync(
        string? oldImageUrl,
        IFormFile newImage,
        string folder,
        CancellationToken cancellationToken = default);
}
