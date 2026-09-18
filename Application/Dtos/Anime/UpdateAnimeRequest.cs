namespace TesteTecnico.Application.Dtos.Anime;
public class UpdateAnimeRequest
{
    public string? Nome { get; set; }
    public string? Descricao { get; set; }
    public int? AnoLancamento { get; set; }
    public int? NumeroEpisodios { get; set; }
    public Guid? DiretorId { get; set; }
}