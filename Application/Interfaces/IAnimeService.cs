using TesteTecnico.Application.Dtos;
using TesteTecnico.Domain.Models;

namespace TesteTecnico.Application.Interfaces;

public interface IAnimeService
{
    Task<IEnumerable<Anime>> GetAllAsync();
    Task<Anime?> GetByIdAsync(int id);
    Task<Anime> AddAsync(CreateAnimeRequest anime);
    Task UpdateAsync(Anime anime);
    Task DeleteAsync(int id);
}