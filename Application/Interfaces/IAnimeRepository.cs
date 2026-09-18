using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Anime;
using TesteTecnico.Domain.Models;

namespace TesteTecnico.Application.Interfaces;

public interface IAnimeRepository
{
    Task<IEnumerable<Anime>> GetAllAsync();
    Task<Anime?> GetByIdAsync(Guid id);
    Task<Anime> AddAsync(CreateAnimeRequest anime);
    Task<Anime?> UpdateAsync(UpdateAnimeRequest request, Guid id);
    Task<Anime?> DeleteAsync(Guid id);
}