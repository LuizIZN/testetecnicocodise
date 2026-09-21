using Microsoft.EntityFrameworkCore;
using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Diretor;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Domain.Models;
using TesteTecnico.Infraestructure.Data;

namespace TesteTecnico.Infraestructure.Repositories;

public sealed class DiretorRepository(AppDbContext context) : IDiretorRepository
{
    private readonly AppDbContext _context = context;

    public async Task<Diretor> AddAsync(CreateDiretorRequest request)
    {
        var newDiretor = new Diretor(Nome: request.Nome, DataNascimento: request.DataNascimento);

        await _context.Diretores.AddAsync(newDiretor);
        await _context.SaveChangesAsync();

        return newDiretor;
    }

    public async Task<IEnumerable<Diretor>> GetAllAsync()
    {
        return await _context.Diretores.ToListAsync();
    }

    public async Task<Diretor?> GetByIdAsync(Guid id)
    {
        return await _context.Diretores.FindAsync(id);
    }

    public async Task<Diretor?> DeleteAsync(Guid id)
    {
        var diretor = await _context.Diretores.FindAsync(id);
        if (diretor == null) return null;

        _context.Diretores.Remove(diretor);
        await _context.SaveChangesAsync();
        return diretor;
    }

    public async Task<Diretor?> UpdateAsync(UpdateDiretorRequest request, Guid id)
    {
        var existingDiretor = await _context.Diretores.FirstOrDefaultAsync(d => d.Id == id);
        if (existingDiretor == null) return null;

        var diretorToUpdate = new Diretor(Nome: request.Nome ?? existingDiretor.Nome, DataNascimento: request.DataNascimento ?? existingDiretor.DataNascimento);

        existingDiretor.Update(diretorToUpdate);
        await _context.SaveChangesAsync();

        return existingDiretor;
    }
}