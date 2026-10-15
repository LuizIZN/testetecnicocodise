using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Diretor;

namespace TesteTecnico.Web.Clients;

public interface IDiretorApiClient
{
    Task<QueryResponse<GetDiretorResponse>> GetAllAsync(QueryDiretorParams parameters, CancellationToken cancellationToken);
    Task<HttpResponseMessage> CreateAsync(CreateDiretorRequest request, CancellationToken cancellationToken);
    Task<HttpResponseMessage> UpdateAsync(Guid id, UpdateDiretorRequest request, CancellationToken cancellationToken);
    Task<HttpResponseMessage> DeleteAsync(Guid id, CancellationToken cancellationToken);
}
