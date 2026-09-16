using TesteTecnico.Application.Dtos;
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

    public async Task<IEnumerable<GetDiretorResponse>> GetAllAsync()
    {
        var diretores = await _diretorRepository.GetAllAsync();
        return diretores.Select(d => MapToGetDiretorResponse(d));
    }

    public async Task<GetDiretorResponse?> GetByIdAsync(Guid id)
    {
        var diretor = await _diretorRepository.GetByIdAsync(id) ?? throw new Exception("Diretor não encontrado");
        return MapToGetDiretorResponse(diretor);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _diretorRepository.DeleteAsync(id);
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