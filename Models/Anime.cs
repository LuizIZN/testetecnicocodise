namespace TesteTecnico.Models;

public class Anime
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string Descricao { get; private set; } = string.Empty;
    public int AnoLancamento { get; private set; }
    public int NumeroEpisodios { get; private set; }

    public Guid DiretorId { get; private set; }
    public Diretor? Diretor {get; private set; }
}