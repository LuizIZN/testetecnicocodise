namespace TesteTecnico.Models;

public class Anime(string Nome, string Descricao, int AnoLancamento, int NumeroEpisodios, Guid DiretorId)
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = Nome;
    public string Descricao { get; private set; } = Descricao;
    public int AnoLancamento { get; private set; } = AnoLancamento;
    public int NumeroEpisodios { get; private set; } = NumeroEpisodios;

    public Guid DiretorId { get; private set; } = DiretorId;
    public Diretor? Diretor {get; private set; }
}