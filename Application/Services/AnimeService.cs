using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Domain.Models;

namespace TesteTecnico.Application.Services;

public sealed class AnimeService(IAnimeRepository animeRepository, IDiretorRepository diretorRepository) : IAnimeService
{
    private readonly IAnimeRepository _animeRepository = animeRepository;
    private readonly IDiretorRepository _diretorRepository = diretorRepository;

    public async Task<IEnumerable<GetAnimeResponse>> GetAllAsync()
    {
        var animes = await _animeRepository.GetAllAsync();

        return animes.Select(a => MapToGetAnimeResponse(a));
    }

    public async Task<GetAnimeResponse?> GetByIdAsync(Guid id)
    {
        var result = await _animeRepository.GetByIdAsync(id) ?? throw new Exception("Anime não encontrado.");

        return MapToGetAnimeResponse(result);
    }

    public async Task<GetAnimeResponse> AddAsync(CreateAnimeRequest request)
    {
        var diretor = await _diretorRepository.GetByIdAsync(request.DiretorId) ?? throw new Exception("Diretor não encontrado.");

        var novoAnime = await _animeRepository.AddAsync(request);
        return MapToGetAnimeResponse(novoAnime);
    }

    public async Task UpdateAsync(Anime anime)
    {
        await _animeRepository.UpdateAsync(anime);
    }

    public async Task DeleteAsync(int id)
    {
        await _animeRepository.DeleteAsync(id);
    }

    private static GetAnimeResponse MapToGetAnimeResponse(Anime anime)
    {
        return new GetAnimeResponse
        {
            Id = anime.Id,
            Nome = anime.Nome,
            AnoLancamento = anime.AnoLancamento,
            NumeroEpisodios = anime.NumeroEpisodios,
            Diretor = anime.Diretor,
            DiretorId = anime.DiretorId,
            Descricao = anime.Descricao
        };
    }
}