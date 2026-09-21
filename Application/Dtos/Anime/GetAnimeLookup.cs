using TesteTecnico.Domain.Models;

namespace TesteTecnico.Application.Dtos;
public sealed class GetAnimeLookup
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int AnoLancamento { get; set; }
    public int NumeroEpisodios { get; set; }
}