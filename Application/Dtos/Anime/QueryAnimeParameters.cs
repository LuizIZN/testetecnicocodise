namespace TesteTecnico.Application.Dtos.Anime;

public class QueryAnimeParameters : QueryParameters
{
    public string? Nome { get; set; }
    public int? AnoLancamentoMin { get; set; }
    public int? AnoLancamentoMax { get; set; }
    public int? NumeroEpisodiosMin { get; set; }
    public int? NumeroEpisodiosMax { get; set; }
}