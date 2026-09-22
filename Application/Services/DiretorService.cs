using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Diretor;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Domain.Models;

namespace TesteTecnico.Application.Services;

public sealed class DiretorService(IDiretorRepository diretorRepository) : IDiretorService
{
    private readonly IDiretorRepository _diretorRepository = diretorRepository;

    public async Task<GetDiretorResponse> AddAsync(CreateDiretorRequest request)
    {
        var diretor = await _diretorRepository.AddAsync(request); 

        return MapToGetDiretorResponse(diretor);
    }

    public async Task<QueryResponse<GetDiretorResponse>> GetAllAsync(QueryDiretorParams queryParameters)
    {
        var diretores = await _diretorRepository.GetAllAsync(queryParameters) ?? throw new Exception("Nenhum diretor encontrado!");
        
        var queryResponse = new QueryResponse<GetDiretorResponse>
        {
            Items = diretores.Items.Select(MapToGetDiretorResponse),
            TotalCount = diretores.TotalCount,
            PageNumber = queryParameters.PageNumber,
            PageSize = queryParameters.PageSize
        };

        return queryResponse;
    }

    public async Task<GetDiretorResponse?> GetByIdAsync(Guid id)
    {
        var diretor = await _diretorRepository.GetByIdAsync(id) ?? throw new Exception("Diretor não encontrado");
        return MapToGetDiretorResponse(diretor);
    }

    public async Task DeleteAsync(Guid id)
    {
        _ = await _diretorRepository.DeleteAsync(id) ?? throw new Exception("Diretor não encontrado para exclusão.");
    }

    public async Task<GetDiretorResponse?> UpdateAsync(UpdateDiretorRequest request, Guid id)
    {
        var updatedDiretor = await _diretorRepository.UpdateAsync(request, id) ?? throw new Exception("Diretor não encontrado para atualização.");
        return MapToGetDiretorResponse(updatedDiretor);
    }

    public async Task<GetAnimeDiretorResponse?> GetDiretorWithAnimesAsync(Guid id)
    {
        var diretor = await _diretorRepository.GetDiretorWithAnimesAsync(id) ?? throw new Exception("Diretor não encontrado.");
        
        return new GetAnimeDiretorResponse
        {
            Id = diretor.Id,
            Nome = diretor.Nome,
            DataNascimento = diretor.DataNascimento,
            Animes = [.. diretor.Animes.Select(a => new GetAnimeLookup
            {
                Id = a.Id,
                Nome = a.Nome,
                Descricao = a.Descricao,
                NumeroEpisodios = a.NumeroEpisodios,
                AnoLancamento = a.AnoLancamento 
            })]
        };
    }

    private static GetDiretorResponse MapToGetDiretorResponse(Diretor diretor)
    {
        return new GetDiretorResponse
        {
            Id = diretor.Id,
            Nome = diretor.Nome,
            DataNascimento = diretor.DataNascimento
        };
    }
}