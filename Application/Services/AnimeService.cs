using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Domain.Models;

namespace TesteTecnico.Application.Services;

public sealed class AnimeService(IAnimeRepository animeRepository) : IAnimeService
{
    private readonly IAnimeRepository _animeRepository = animeRepository;

    public async Task<IEnumerable<Anime>> GetAllAsync()
    {
        return await _animeRepository.GetAllAsync();
    }

    public async Task<Anime?> GetByIdAsync(int id)
    {
        return await _animeRepository.GetByIdAsync(id);
    }

    public async Task<Anime> AddAsync(CreateAnimeRequest anime)
    {
        return await _animeRepository.AddAsync(anime);
    }

    public async Task UpdateAsync(Anime anime)
    {
        await _animeRepository.UpdateAsync(anime);
    }

    public async Task DeleteAsync(int id)
    {
        await _animeRepository.DeleteAsync(id);
    }
}