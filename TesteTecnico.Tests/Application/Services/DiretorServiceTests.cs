using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Diretor;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Services;
using TesteTecnico.Domain.Models;
using Xunit;

namespace TesteTecnico.Tests.Application.Services;

public sealed class DiretorServiceTests
{
    [Fact]
    public async Task AddAsync_DelegatesToRepositoryAndMapsResponse()
    {
        var repository = new FakeDiretorRepository();
        var service = new DiretorService(repository);
        var request = new CreateDiretorRequest
        {
            Nome = "Hayao Miyazaki",
            DataNascimento = new DateOnly(1941, 1, 5)
        };

        var result = await service.AddAsync(request);

        Assert.Equal(request.Nome, result.Nome);
        Assert.Equal(request.DataNascimento, result.DataNascimento);
        Assert.Equal(repository.AddedDiretor!.Id, result.Id);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDirectors()
    {
        var diretor = new Diretor("Makoto Shinkai", new DateOnly(1973, 2, 9));
        var repository = new FakeDiretorRepository { Directors = [diretor] };
        var service = new DiretorService(repository);

        var result = (await service.GetAllAsync(new QueryDiretorParams())).Items.Single();

        Assert.Equal(diretor.Id, result.Id);
        Assert.Equal(diretor.Nome, result.Nome);
        Assert.Equal(diretor.DataNascimento, result.DataNascimento);
    }

    [Fact]
    public async Task GetByIdAsync_WhenDirectorDoesNotExist_ThrowsExpectedException()
    {
        var service = new DiretorService(new FakeDiretorRepository());

        var exception = await Assert.ThrowsAsync<Exception>(() => service.GetByIdAsync(Guid.NewGuid()));

        Assert.Equal("Diretor não encontrado", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenDirectorDoesNotExist_ThrowsExpectedException()
    {
        var service = new DiretorService(new FakeDiretorRepository());
        var request = new UpdateDiretorRequest { Nome = "Novo nome" };

        var exception = await Assert.ThrowsAsync<Exception>(() => service.UpdateAsync(request, Guid.NewGuid()));

        Assert.Equal("Diretor não encontrado para atualização.", exception.Message);
    }

    [Fact]
    public async Task GetDiretorWithAnimesAsync_MapsDirectorAndAnimes()
    {
        var diretor = new Diretor("Satoshi Kon", new DateOnly(1963, 10, 12));
        var anime = new Anime("Perfect Blue", "Suspense psicológico", 1997, 1, diretor.Id);
        diretor.Animes.Add(anime);
        var service = new DiretorService(new FakeDiretorRepository { DiretorWithAnimes = diretor });

        var result = await service.GetDiretorWithAnimesAsync(diretor.Id);

        Assert.NotNull(result);
        Assert.Equal(diretor.Nome, result.Nome);
        Assert.Equal(diretor.DataNascimento, result.DataNascimento);
        var animeResult = Assert.Single(result.Animes);
        Assert.Equal(anime.Nome, animeResult.Nome);
        Assert.Equal(anime.NumeroEpisodios, animeResult.NumeroEpisodios);
    }

    private sealed class FakeDiretorRepository : IDiretorRepository
    {
        public IEnumerable<Diretor> Directors { get; init; } = [];
        public Diretor? DiretorWithAnimes { get; init; }
        public Diretor? AddedDiretor { get; private set; }

        public Task<Diretor> AddAsync(CreateDiretorRequest request)
        {
            AddedDiretor = new Diretor(request.Nome, request.DataNascimento);
            return Task.FromResult(AddedDiretor);
        }

        public Task<GetAllType<Diretor>> GetAllAsync(QueryDiretorParams queryParameters) => Task.FromResult(new GetAllType<Diretor>
        {
            Items = Directors,
            TotalCount = Directors.Count()
        });

        public Task<Diretor?> GetByIdAsync(Guid id) =>
            Task.FromResult(Directors.FirstOrDefault(diretor => diretor.Id == id));

        public Task<Diretor?> UpdateAsync(UpdateDiretorRequest request, Guid id) =>
            Task.FromResult<Diretor?>(null);

        public Task<Diretor?> DeleteAsync(Guid id) =>
            Task.FromResult<Diretor?>(null);

        public Task<Diretor?> GetDiretorWithAnimesAsync(Guid id) =>
            Task.FromResult(DiretorWithAnimes);
    }
}