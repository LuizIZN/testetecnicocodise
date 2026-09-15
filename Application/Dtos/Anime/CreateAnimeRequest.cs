namespace TesteTecnico.Application.Dtos;

public sealed class CreateAnimeRequest
{
    public string Nome { get; set; } = string.Empty;
    public string Descricao { get; set; } = string.Empty;
    public int AnoLancamento { get; set; }
    public int NumeroEpisodios { get; set; }
    public Guid DiretorId { get; set; }
}