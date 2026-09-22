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

    public async Task<GetAllType<Anime>> GetAllAsync(QueryAnimeParameters queryParameters)
    {
        var query = _context.Animes.AsQueryable();

        if (!string.IsNullOrEmpty(queryParameters.Nome))
        {
            query = query.Where(a => a.Nome.ToLower().Contains(queryParameters.Nome.ToLower()));
        }

        if (queryParameters.AnoLancamentoMin.HasValue)
        {
            query = query.Where(a => a.AnoLancamento >= queryParameters.AnoLancamentoMin.Value);
        }

        if (queryParameters.AnoLancamentoMax.HasValue)
        {
            query = query.Where(a => a.AnoLancamento <= queryParameters.AnoLancamentoMax.Value);
        }

        if (queryParameters.NumeroEpisodiosMin.HasValue)
        {
            query = query.Where(a => a.NumeroEpisodios >= queryParameters.NumeroEpisodiosMin.Value);
        }

        if (queryParameters.NumeroEpisodiosMax.HasValue)
        {
            query = query.Where(a => a.NumeroEpisodios <= queryParameters.NumeroEpisodiosMax.Value);
        }

        var pageNumber = Math.Max(queryParameters.PageNumber, 1);
        var pageSize = Math.Clamp(queryParameters.PageSize, 1, 100);

        var totalCount = await query.CountAsync();

        var items = await query.Include(a => a.Diretor)
            .OrderBy(a => a.Nome)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var response = new GetAllType<Anime>
        {
            Items = items,
            TotalCount = totalCount,
        };

        return response;
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