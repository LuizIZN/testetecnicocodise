using System.Net.Http.Json;
using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Anime;

namespace TesteTecnico.Web.Clients;

public sealed class AnimeApiClient(HttpClient httpClient) : IAnimeApiClient
{
    public async Task<QueryResponse<GetAnimeResponse>> GetAllAsync(QueryAnimeParameters parameters, CancellationToken cancellationToken)
    {
        var query = new List<string>();
        AddQuery(query, "Nome", parameters.Nome);
        AddQuery(query, "AnoLancamentoMin", parameters.AnoLancamentoMin);
        AddQuery(query, "AnoLancamentoMax", parameters.AnoLancamentoMax);
        AddQuery(query, "NumeroEpisodiosMin", parameters.NumeroEpisodiosMin);
        AddQuery(query, "NumeroEpisodiosMax", parameters.NumeroEpisodiosMax);
        AddQuery(query, "PageNumber", parameters.PageNumber);
        AddQuery(query, "PageSize", parameters.PageSize);

        var result = await httpClient.GetFromJsonAsync<QueryResponse<GetAnimeResponse>>(
            $"api/anime?{string.Join("&", query)}", cancellationToken)
            ?? throw new InvalidOperationException("A API retornou uma resposta vazia.");

        foreach (var anime in result.Items)
        {
            if (Uri.TryCreate(anime.ImageUrl, UriKind.Relative, out var relativeImage))
            {
                anime.ImageUrl = new Uri(httpClient.BaseAddress!, relativeImage).ToString();
            }
        }

        return result;
    }

    public Task<HttpResponseMessage> CreateAsync(CreateAnimeRequest request, CancellationToken cancellationToken) =>
        SendFormAsync(HttpMethod.Post, "api/anime", request, cancellationToken);

    public Task<HttpResponseMessage> UpdateAsync(Guid id, UpdateAnimeRequest request, CancellationToken cancellationToken) =>
        SendFormAsync(HttpMethod.Patch, $"api/anime/{id}", request, cancellationToken);

    public Task<HttpResponseMessage> DeleteAsync(Guid id, CancellationToken cancellationToken) =>
        httpClient.DeleteAsync($"api/anime/{id}", cancellationToken);

    private static void AddQuery(List<string> query, string name, object? value)
    {
        if (value is not null && value.ToString() is { Length: > 0 } text)
        {
            query.Add($"{Uri.EscapeDataString(name)}={Uri.EscapeDataString(text)}");
        }
    }

    private async Task<HttpResponseMessage> SendFormAsync<T>(
        HttpMethod method, string path, T request, CancellationToken cancellationToken)
    {
        using var content = new MultipartFormDataContent();
        foreach (var property in typeof(T).GetProperties())
        {
            var value = property.GetValue(request);
            if (value is null || property.Name == "ImageUrl")
            {
                continue;
            }

            if (value is IFormFile file)
            {
                content.Add(new StreamContent(file.OpenReadStream()), property.Name, file.FileName);
            }
            else
            {
                content.Add(new StringContent(value.ToString()!), property.Name);
            }
        }

        using var message = new HttpRequestMessage(method, path) { Content = content };
        return await httpClient.SendAsync(message, cancellationToken);
    }
}
