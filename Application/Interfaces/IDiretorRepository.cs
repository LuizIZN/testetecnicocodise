using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Diretor;
using TesteTecnico.Domain.Models;

namespace TesteTecnico.Application.Interfaces;

public interface IDiretorRepository
{
    Task<GetAllType<Diretor>> GetAllAsync(QueryDiretorParams queryParameters);
    Task<Diretor?> GetByIdAsync(Guid id);
    Task<Diretor> AddAsync(CreateDiretorRequest diretor);
    Task<Diretor?> UpdateAsync(UpdateDiretorRequest diretor, Guid id);
    Task<Diretor?> DeleteAsync(Guid id);
    Task<Diretor?> GetDiretorWithAnimesAsync(Guid id);
}