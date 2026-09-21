using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Anime;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Services;
using TesteTecnico.Application.Dtos.Diretor;
using TesteTecnico.Domain.Models;
using Xunit;

namespace TesteTecnico.Tests.Application.Services;

public sealed class AnimeServiceTests
{
    [Fact]
    public async Task GetAllAsync_ReturnsMappedAnimes()
    {
        var diretor = CreateDirector();
        var anime = CreateAnime(diretor);
        var service = new AnimeService(
            new FakeAnimeRepository { Animes = [anime] },
            new FakeDiretorRepository());

        var result = (await service.GetAllAsync()).Single();

        Assert.Equal(anime.Id, result.Id);
        Assert.Equal(anime.Nome, result.Nome);
        Assert.Equal(anime.Descricao, result.Descricao);
        Assert.Equal(anime.AnoLancamento, result.AnoLancamento);
        Assert.Equal(anime.NumeroEpisodios, result.NumeroEpisodios);
        Assert.Equal(diretor.Id, result.Diretor.Id);
        Assert.Equal(diretor.Nome, result.Diretor.Nome);
    }

    [Fact]
    public async Task GetByIdAsync_WhenAnimeDoesNotExist_ThrowsExpectedException()
    {
        var service = CreateService();

        var exception = await Assert.ThrowsAsync<Exception>(() => service.GetByIdAsync(Guid.NewGuid()));

        Assert.Equal("Anime não encontrado.", exception.Message);
    }

    [Fact]
    public async Task AddAsync_WithValidRequest_ValidatesDirectorAndMapsResponse()
    {
        var diretor = CreateDirector();
        var anime = CreateAnime(diretor);
        var animeRepository = new FakeAnimeRepository { AnimeToAdd = anime };
        var diretorRepository = new FakeDiretorRepository { Director = diretor };
        var service = new AnimeService(animeRepository, diretorRepository);
        var request = new CreateAnimeRequest
        {
            Nome = anime.Nome,
            Descricao = anime.Descricao,
            AnoLancamento = anime.AnoLancamento,
            NumeroEpisodios = anime.NumeroEpisodios,
            DiretorId = diretor.Id
        };

        var result = await service.AddAsync(request);

        Assert.Equal(anime.Id, result.Id);
        Assert.Equal(diretor.Id, diretorRepository.RequestedId);
        Assert.Same(request, animeRepository.AddedRequest);
    }

    [Fact]
    public async Task AddAsync_WithMissingNameOrDescription_ThrowsValidationException()
    {
        var service = CreateService();
        var request = ValidCreateRequest();
        request.Nome = " ";

        var exception = await Assert.ThrowsAsync<Exception>(() => service.AddAsync(request));

        Assert.Equal("Os campos nome e descrição são obrigatórios!", exception.Message);
    }

    [Fact]
    public async Task AddAsync_WithNonPositiveNumbers_ThrowsValidationException()
    {
        var service = CreateService();
        var request = ValidCreateRequest();
        request.NumeroEpisodios = 0;

        var exception = await Assert.ThrowsAsync<Exception>(() => service.AddAsync(request));

        Assert.Equal("Os campos ano de lançamento e número de episódios devem ser números maiores que zero!", exception.Message);
    }

    [Fact]
    public async Task AddAsync_WithFutureYear_ThrowsValidationException()
    {
        var service = CreateService();
        var request = ValidCreateRequest();
        request.AnoLancamento = DateTime.Now.Year + 1;

        var exception = await Assert.ThrowsAsync<Exception>(() => service.AddAsync(request));

        Assert.Equal("Não é possível cadastrar um ano de lançamento maior que o ano atual!", exception.Message);
    }

    [Fact]
    public async Task AddAsync_WithUnknownDirector_ThrowsExpectedException()
    {
        var service = CreateService();

        var exception = await Assert.ThrowsAsync<Exception>(() => service.AddAsync(ValidCreateRequest()));

        Assert.Equal("Diretor não encontrado.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenAnimeDoesNotExist_ThrowsExpectedException()
    {
        var service = CreateService();
        var request = new UpdateAnimeRequest { AnoLancamento = 2000, NumeroEpisodios = 12 };

        var exception = await Assert.ThrowsAsync<Exception>(() => service.UpdateAsync(request, Guid.NewGuid()));

        Assert.Equal("Anime não encontrado!", exception.Message);
    }

    [Fact]
    public async Task DeleteAsync_WhenAnimeExists_ReturnsMappedAnime()
    {
        var diretor = CreateDirector();
        var anime = CreateAnime(diretor);
        var service = CreateService(new FakeAnimeRepository { AnimeToDelete = anime });

        var result = await service.DeleteAsync(anime.Id);

        Assert.NotNull(result);
        Assert.Equal(anime.Id, result.Id);
        Assert.Equal(anime.Nome, result.Nome);
    }

    private static AnimeService CreateService(FakeAnimeRepository? animeRepository = null)
    {
        return new AnimeService(animeRepository ?? new FakeAnimeRepository(), new FakeDiretorRepository());
    }

    private static Diretor CreateDirector() => new("Hayao Miyazaki", new DateOnly(1941, 1, 5));

    private static Anime CreateAnime(Diretor diretor)
    {
        var anime = new Anime("Meu Vizinho Totoro", "Uma aventura", 1988, 1, diretor.Id);
        typeof(Anime).GetProperty(nameof(Anime.Diretor))!.SetValue(anime, diretor);
        return anime;
    }

    private static CreateAnimeRequest ValidCreateRequest() => new()
    {
        Nome = "Meu Vizinho Totoro",
        Descricao = "Uma aventura",
        AnoLancamento = 1988,
        NumeroEpisodios = 1,
        DiretorId = Guid.NewGuid()
    };

    private sealed class FakeAnimeRepository : IAnimeRepository
    {
        public IEnumerable<Anime> Animes { get; init; } = [];
        public Anime? AnimeToAdd { get; init; }
        public Anime? AnimeToUpdate { get; init; }
        public Anime? AnimeToDelete { get; init; }
        public CreateAnimeRequest? AddedRequest { get; private set; }

        public Task<IEnumerable<Anime>> GetAllAsync(QueryAnimeParameters queryParameters) => Task.FromResult(Animes);

        public Task<Anime?> GetByIdAsync(Guid id) =>
            Task.FromResult(Animes.FirstOrDefault(anime => anime.Id == id));

        public Task<Anime> AddAsync(CreateAnimeRequest request)
        {
            AddedRequest = request;
            return Task.FromResult(AnimeToAdd!);
        }

        public Task<Anime?> UpdateAsync(UpdateAnimeRequest request, Guid id) =>
            Task.FromResult(AnimeToUpdate);

        public Task<Anime?> DeleteAsync(Guid id) =>
            Task.FromResult(AnimeToDelete);
    }

    private sealed class FakeDiretorRepository : IDiretorRepository
    {
        public Diretor? Director { get; init; }
        public Guid RequestedId { get; private set; }

        public Task<Diretor?> GetByIdAsync(Guid id)
        {
            RequestedId = id;
            return Task.FromResult(Director);
        }

        public Task<IEnumerable<Diretor>> GetAllAsync() => Task.FromResult<IEnumerable<Diretor>>([]);
        public Task<Diretor> AddAsync(CreateDiretorRequest request) => throw new NotSupportedException();
        public Task<Diretor?> UpdateAsync(UpdateDiretorRequest request, Guid id) => throw new NotSupportedException();
        public Task<Diretor?> DeleteAsync(Guid id) => throw new NotSupportedException();
        public Task<Diretor?> GetDiretorWithAnimesAsync(Guid id) => throw new NotSupportedException();
    }
}