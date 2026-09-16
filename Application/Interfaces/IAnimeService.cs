using TesteTecnico.Application.Dtos;
using TesteTecnico.Domain.Models;

namespace TesteTecnico.Application.Interfaces;

public interface IAnimeService
{
    Task<IEnumerable<GetAnimeResponse>> GetAllAsync();
    Task<GetAnimeResponse?> GetByIdAsync(Guid id);
    Task<GetAnimeResponse> AddAsync(CreateAnimeRequest anime);
    Task UpdateAsync(Anime anime);
    Task DeleteAsync(int id);
}