using Microsoft.EntityFrameworkCore;
using TesteTecnico.Application.Dtos.Diretor;
using TesteTecnico.Domain.Models;
using TesteTecnico.Infraestructure.Data;
using TesteTecnico.Infraestructure.Repositories;
using Xunit;

namespace TesteTecnico.Tests.Infrastructure.Repositories;

public sealed class DiretorRepositoryTests
{
    [Fact]
    public async Task GetAllAsync_FiltersByNameIgnoringCase()
    {
        await using var context = CreateContext();
        var repository = new DiretorRepository(context);
        await SeedAsync(context);

        var result = await repository.GetAllAsync(new QueryDiretorParams { Nome = "MIYAZAKI" });

        var diretor = Assert.Single(result.Items);
        Assert.Equal("Hayao Miyazaki", diretor.Nome);
        Assert.Equal(1, result.TotalCount);
    }

    [Fact]
    public async Task GetAllAsync_AppliesPaginationAndOrdersByName()
    {
        await using var context = CreateContext();
        var repository = new DiretorRepository(context);
        await SeedAsync(context);

        var result = await repository.GetAllAsync(new QueryDiretorParams
        {
            PageNumber = 2,
            PageSize = 1
        });

        Assert.Equal(4, result.TotalCount);
        var diretor = Assert.Single(result.Items);
        Assert.Equal("Isao Takahata", diretor.Nome);
    }

    [Fact]
    public async Task GetAllAsync_CalculatesTotalCountAfterApplyingNameFilter()
    {
        await using var context = CreateContext();
        var repository = new DiretorRepository(context);
        await SeedAsync(context);

        var result = await repository.GetAllAsync(new QueryDiretorParams
        {
            Nome = "a"
        });

        Assert.Equal(4, result.TotalCount);
        Assert.Equal(4, result.Items.Count());
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
        context.Diretores.AddRange(
            new Diretor("Hayao Miyazaki", new DateOnly(1941, 1, 5)),
            new Diretor("Makoto Shinkai", new DateOnly(1973, 2, 9)),
            new Diretor("Satoshi Kon", new DateOnly(1963, 10, 12)),
            new Diretor("Isao Takahata", new DateOnly(1935, 10, 29)));

        await context.SaveChangesAsync();
    }
}
