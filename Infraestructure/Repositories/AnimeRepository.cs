using Microsoft.EntityFrameworkCore;
using TesteTecnico.Infraestructure.Data;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Domain.Models;
using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Anime;

namespace TesteTecnico.Infraestructure.Repositories;

public sealed class AnimeRepository(AppDbContext context) : IAnimeRepository
{
    private readonly AppDbContext _context = context;

    public async Task<IEnumerable<Anime>> GetAllAsync()
    {
        return await _context.Animes.Include(a => a.Diretor).ToListAsync();
    }

    public async Task<Anime?> GetByIdAsync(Guid id)
    {
        return await _context.Animes.Include(a => a.Diretor).FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Anime> AddAsync(CreateAnimeRequest anime)
    {
        var newAnime = new Anime(Nome: anime.Nome, Descricao: anime.Descricao, AnoLancamento: anime.AnoLancamento, DiretorId: anime.DiretorId, NumeroEpisodios: anime.NumeroEpisodios);

        await _context.Animes.AddAsync(newAnime);
        await _context.SaveChangesAsync();

        return newAnime;
    }

    public async Task<Anime?> UpdateAsync(UpdateAnimeRequest anime, Guid id)
    {
        var existingAnime = await _context.Animes.FirstOrDefaultAsync(a => a.Id == id);

        if (existingAnime is null) return null;

        var updatedAnime = new Anime(
            Nome: anime.Nome ?? existingAnime.Nome,
            Descricao: anime.Descricao ?? existingAnime.Descricao,
            AnoLancamento: anime.AnoLancamento ?? existingAnime.AnoLancamento,
            DiretorId: anime.DiretorId ?? existingAnime.DiretorId,
            NumeroEpisodios: anime.NumeroEpisodios ?? existingAnime.NumeroEpisodios
        );
        existingAnime.Update(updatedAnime);
        await _context.SaveChangesAsync();
        
        return existingAnime;
    }

    public async Task<Anime?> DeleteAsync(Guid id)
    {
        var anime = await _context.Animes.FindAsync(id);
        if (anime == null) return null;

        _context.Animes.Remove(anime);
        await _context.SaveChangesAsync();

        return anime;
    }
}