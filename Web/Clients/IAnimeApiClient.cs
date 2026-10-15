using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Anime;

namespace TesteTecnico.Web.Clients;

public interface IAnimeApiClient
{
    Task<QueryResponse<GetAnimeResponse>> GetAllAsync(QueryAnimeParameters parameters, CancellationToken cancellationToken);
    Task<HttpResponseMessage> CreateAsync(CreateAnimeRequest request, CancellationToken cancellationToken);
    Task<HttpResponseMessage> UpdateAsync(Guid id, UpdateAnimeRequest request, CancellationToken cancellationToken);
    Task<HttpResponseMessage> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
