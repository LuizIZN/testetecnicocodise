using TesteTecnico.Domain.Models;

namespace TesteTecnico.Application.Dtos;
public sealed class GetAnimeResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int AnoLancamento { get; set; }
    public Guid DiretorId { get; set; }
    public Diretor? Diretor { get; set; }
    public int NumeroEpisodios { get; set; }
}