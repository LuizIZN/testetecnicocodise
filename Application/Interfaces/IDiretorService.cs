using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Diretor;
using TesteTecnico.Domain.Models;

namespace TesteTecnico.Application.Interfaces;

public interface IDiretorService
{
    Task<IEnumerable<GetDiretorResponse>> GetAllAsync();
    Task<GetDiretorResponse?> GetByIdAsync(Guid id);
    Task<GetDiretorResponse> AddAsync(CreateDiretorRequest diretor);
    Task<GetDiretorResponse?> UpdateAsync(UpdateDiretorRequest diretor, Guid id);
    Task<GetDiretorResponse?> DeleteAsync(Guid id);
}