using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Anime;

namespace TesteTecnico.Application.Interfaces;

public interface IAnimeService
{
    Task<IEnumerable<GetAnimeResponse>> GetAllAsync(QueryAnimeParameters queryParameters);
    Task<GetAnimeResponse?> GetByIdAsync(Guid id);
    Task<GetAnimeResponse> AddAsync(CreateAnimeRequest anime);
    Task<GetAnimeResponse?> UpdateAsync(UpdateAnimeRequest request, Guid id);
    Task<GetAnimeResponse?> DeleteAsync(Guid id);
}