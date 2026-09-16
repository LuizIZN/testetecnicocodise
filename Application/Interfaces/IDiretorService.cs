using TesteTecnico.Application.Dtos;
using TesteTecnico.Domain.Models;

namespace TesteTecnico.Application.Interfaces;

public interface IDiretorService
{
    Task<IEnumerable<GetDiretorResponse>> GetAllAsync();
    Task<GetDiretorResponse?> GetByIdAsync(Guid id);
    Task<GetDiretorResponse> AddAsync(CreateDiretorRequest diretor);
    //Task UpdateAsync(UpdateDiretorRequest diretor);
    Task DeleteAsync(Guid id);
}