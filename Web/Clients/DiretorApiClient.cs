using System.Net.Http.Json;
using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Diretor;

namespace TesteTecnico.Web.Clients;

public sealed class DiretorApiClient(HttpClient httpClient) : IDiretorApiClient
{
    public async Task<QueryResponse<GetDiretorResponse>> GetAllAsync(QueryDiretorParams parameters, CancellationToken cancellationToken)
    {
        var query = string.IsNullOrWhiteSpace(parameters.Nome)
            ? string.Empty
            : $"?Nome={Uri.EscapeDataString(parameters.Nome)}";

        using var response = await httpClient.GetAsync($"api/diretor{query}", cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var details = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException(
                $"A API retornou {(int)response.StatusCode} ({response.ReasonPhrase}). " +
                $"Detalhes: {details}");
        }

        return await response.Content.ReadFromJsonAsync<QueryResponse<GetDiretorResponse>>(
            cancellationToken)
            ?? throw new InvalidOperationException("A API retornou uma resposta vazia.");
    }

    public Task<HttpResponseMessage> CreateAsync(CreateDiretorRequest request, CancellationToken cancellationToken) =>
        httpClient.PostAsJsonAsync("api/diretor", request, cancellationToken);

    public Task<HttpResponseMessage> UpdateAsync(Guid id, UpdateDiretorRequest request, CancellationToken cancellationToken) =>
        httpClient.PatchAsJsonAsync($"api/diretor/{id}", request, cancellationToken);

    public Task<HttpResponseMessage> DeleteAsync(Guid id, CancellationToken cancellationToken) =>
        httpClient.DeleteAsync($"api/diretor/{id}", cancellationToken);
}
