using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Anime;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Domain.Models;

namespace TesteTecnico.Application.Services;

public sealed class AnimeService(IAnimeRepository animeRepository, IDiretorRepository diretorRepository) : IAnimeService
{
    private readonly IAnimeRepository _animeRepository = animeRepository;
    private readonly IDiretorRepository _diretorRepository = diretorRepository;

    public async Task<QueryResponse<GetAnimeResponse>> GetAllAsync(QueryAnimeParameters queryParameters)
    {
        var animes = await _animeRepository.GetAllAsync(queryParameters) ?? throw new Exception("Nenhum anime encontrado!");

        var queryResponse = new QueryResponse<GetAnimeResponse>
        {
            Items = animes.Items.Select(MapToGetAnimeResponse),
            TotalCount = animes.TotalCount,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize
        };

        return queryResponse;
    }

    public async Task<GetAnimeResponse?> GetByIdAsync(Guid id)
    {
        var result = await _animeRepository.GetByIdAsync(id) ?? throw new Exception("Anime não encontrado.");

        return MapToGetAnimeResponse(result);
    }

    public async Task<GetAnimeResponse> AddAsync(CreateAnimeRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nome) || string.IsNullOrWhiteSpace(request.Descricao))
        {
            throw new Exception("Os campos nome e descrição são obrigatórios!");
        }

        if (request.AnoLancamento <= 0 || request.NumeroEpisodios <= 0)
        {
            throw new Exception("Os campos ano de lançamento e número de episódios devem ser números maiores que zero!");
        }

        if (request.AnoLancamento > DateTime.Now.Year)
        {
            throw new Exception("Não é possível cadastrar um ano de lançamento maior que o ano atual!");
        }

        _ = await _diretorRepository.GetByIdAsync(request.DiretorId) ?? throw new Exception("Diretor não encontrado.");

        var novoAnime = await _animeRepository.AddAsync(request);
        return MapToGetAnimeResponse(novoAnime);
    }

    public async Task<GetAnimeResponse?> UpdateAsync(UpdateAnimeRequest request, Guid id)
    {
        if (request.AnoLancamento <= 0 || request.NumeroEpisodios <= 0)
        {
            throw new Exception("Os campos ano de lançamento e número de episódios devem ser números maiores que zero!");
        }

        if (request.AnoLancamento > DateTime.Now.Year)
        {
            throw new Exception("Não é possível cadastrar um ano de lançamento maior que o ano atual!");
        }

        var updatedAnime = await _animeRepository.UpdateAsync(request, id) ?? throw new Exception("Anime não encontrado!");
        return MapToGetAnimeResponse(updatedAnime);
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await _animeRepository.DeleteAsync(id) ?? throw new Exception("Anime não encontrado!");
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