using TesteTecnico.Application.Dtos;

namespace TesteTecnico.Api.ViewModels;

public sealed class AnimeViewModel
{
    public QueryResponse<GetAnimeResponse> Animes { get; init; } = new();
    public QueryResponse<GetDiretorResponse> Diretores { get; init; } = new();
}
