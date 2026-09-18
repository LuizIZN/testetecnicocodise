using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Anime;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Domain.Models;

namespace TesteTecnico.Application.Services;

public sealed class AnimeService(IAnimeRepository animeRepository, IDiretorRepository diretorRepository) : IAnimeService
{
    private readonly IAnimeRepository _animeRepository = animeRepository;
    private readonly IDiretorRepository _diretorRepository = diretorRepository;

    public async Task<IEnumerable<GetAnimeResponse>> GetAllAsync()
    {
        var animes = await _animeRepository.GetAllAsync() ?? throw new Exception("Nenhum anime encontrado!");

        return animes.Select(MapToGetAnimeResponse);
    }

    public async Task<GetAnimeResponse?> GetByIdAsync(Guid id)
    {
        var result = await _animeRepository.GetByIdAsync(id) ?? throw new Exception("Anime não encontrado.");

        return MapToGetAnimeResponse(result);
    }

    public async Task<GetAnimeResponse> AddAsync(CreateAnimeRequest request)
    {
        _ = await _diretorRepository.GetByIdAsync(request.DiretorId) ?? throw new Exception("Diretor não encontrado.");

        var novoAnime = await _animeRepository.AddAsync(request);
        return MapToGetAnimeResponse(novoAnime);
    }

    public async Task<GetAnimeResponse?> UpdateAsync(UpdateAnimeRequest request, Guid id)
    {
        var updatedAnime = await _animeRepository.UpdateAsync(request, id) ?? throw new Exception("Anime não encontrado!");
        return MapToGetAnimeResponse(updatedAnime);
    }

    public async Task<GetAnimeResponse?> DeleteAsync(Guid id)
    {
        var deletedAnime = await _animeRepository.DeleteAsync(id) ?? throw new Exception("Anime não encontrado!");
        return MapToGetAnimeResponse(deletedAnime);
    }

    private static GetAnimeResponse MapToGetAnimeResponse(Anime anime)
    {
        return new GetAnimeResponse
        {
            Id = anime.Id,
            Nome = anime.Nome,
            AnoLancamento = anime.AnoLancamento,
            NumeroEpisodios = anime.NumeroEpisodios,
            Diretor = new GetDiretorResponse
            {
                Id = anime.Diretor.Id,
                Nome = anime.Diretor.Nome,
                DataNascimento = anime.Diretor.DataNascimento
            },
            Descricao = anime.Descricao
        };
    }
}