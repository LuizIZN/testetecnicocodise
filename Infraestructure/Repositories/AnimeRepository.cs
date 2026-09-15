using Microsoft.EntityFrameworkCore;
using TesteTecnico.Infraestructure.Data;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Domain.Models;

namespace TesteTecnico.Infraestructure.Repositories;

public sealed class AnimeRepository(AppDbContext context) : IAnimeRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Anime>> GetAllAsync()
    {
        return await _context.Animes.ToListAsync();
    }

    public async Task<Anime?> GetByIdAsync(int id)
    {
        return await _context.Animes.FindAsync(id);
    }

    public async Task AddAsync(Anime anime)
    {
        await _context.Animes.AddAsync(anime);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Anime anime)
    {
        _context.Animes.Update(anime);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var anime = await _context.Animes.FindAsync(id);
        if (anime != null)
        {
            _context.Animes.Remove(anime);
            await _context.SaveChangesAsync();
        }
    }
}