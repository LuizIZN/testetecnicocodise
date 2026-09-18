using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Diretor;
using TesteTecnico.Domain.Models;

namespace TesteTecnico.Application.Interfaces;

public interface IDiretorRepository
{
    Task<IEnumerable<Diretor>> GetAllAsync();
    Task<Diretor?> GetByIdAsync(Guid id);
    Task<Diretor> AddAsync(CreateDiretorRequest diretor);
    Task UpdateAsync(UpdateDiretorRequest diretor, Guid id);
    Task DeleteAsync(Guid id);
}