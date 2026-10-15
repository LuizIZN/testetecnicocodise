using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Anime;

namespace TesteTecnico.Web.ViewModels;

public sealed class AnimeViewModel
{
    public QueryResponse<GetAnimeResponse> Animes { get; init; } = new();
    public QueryResponse<GetDiretorResponse> Diretores { get; init; } = new();
}
