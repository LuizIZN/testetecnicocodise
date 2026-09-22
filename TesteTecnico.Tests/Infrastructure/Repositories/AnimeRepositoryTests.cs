using Microsoft.EntityFrameworkCore;
using TesteTecnico.Application.Dtos.Anime;
using TesteTecnico.Domain.Models;
using TesteTecnico.Infraestructure.Data;
using TesteTecnico.Infraestructure.Repositories;
using Xunit;

namespace TesteTecnico.Tests.Infrastructure.Repositories;

public sealed class AnimeRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_FiltersByNameIgnoringCase()
    {
        await using var context = CreateContext();
        var repository = new AnimeRepository(context);
        await SeedAsync(context);

        var result = await repository.GetAllAsync(new QueryAnimeParameters { Nome = "TOTORO" });

        var anime = Assert.Single(result.Items);
        Assert.Equal("Meu Vizinho Totoro", anime.Nome);
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task GetAllAsync_FiltersByReleaseYearAndEpisodeRanges()
    {
        await using var context = CreateContext();
        var repository = new AnimeRepository(context);
        await SeedAsync(context);

        var result = await repository.GetAllAsync(new QueryAnimeParameters
        {
            AnoLancamentoMin = 1990,
            AnoLancamentoMax = 2000,
            NumeroEpisodiosMin = 10,
            NumeroEpisodiosMax = 30
        });

        var anime = Assert.Single(result.Items);
        Assert.Equal("Princesa Mononoke", anime.Nome);
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task GetAllAsync_AppliesFiltersBeforePaginationAndOrdersByName()
    {
        await using var context = CreateContext();
        var repository = new AnimeRepository(context);
        await SeedAsync(context);

        var result = await repository.GetAllAsync(new QueryAnimeParameters
        {
            PageNumber = 2,
            PageSize = 1,
            AnoLancamentoMin = 1980
        });

        Assert.Equal(4, result.TotalCount);
        var anime = Assert.Single(result.Items);
        Assert.Equal("Meu Vizinho Totoro", anime.Nome);
    }

    private static AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    private static async Task SeedAsync(AppDbContext context)
    {
        var diretor = new Diretor("Hayao Miyazaki", new DateOnly(1941, 1, 5));
        context.Diretores.Add(diretor);
        context.Animes.AddRange(
            new Anime("Meu Vizinho Totoro", "Uma aventura", 1988, 1, diretor.Id),
            new Anime("A Viagem de Chihiro", "Uma viagem", 2001, 15, diretor.Id),
            new Anime("Princesa Mononoke", "Uma lenda", 1997, 24, diretor.Id),
            new Anime("Nausicaa", "Um reino", 1984, 8, diretor.Id));

        await context.SaveChangesAsync();
    }
}
