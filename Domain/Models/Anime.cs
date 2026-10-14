namespace TesteTecnico.Domain.Models;

public class Anime(string Nome, string Descricao, int AnoLancamento, int NumeroEpisodios, Guid DiretorId, string? ImageUrl = null)
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = Nome;
    public string Descricao { get; private set; } = Descricao;
    public int AnoLancamento { get; private set; } = AnoLancamento;
    public int NumeroEpisodios { get; private set; } = NumeroEpisodios;
    public string? ImageUrl { get; private set; } = ImageUrl;

    public Guid DiretorId { get; private set; } = DiretorId;
    public Diretor Diretor {get; private set; } = null!;

    public void Update(Anime updatedAnime)
    {
        Nome = updatedAnime.Nome;
        Descricao = updatedAnime.Descricao;
        AnoLancamento = updatedAnime.AnoLancamento;
        NumeroEpisodios = updatedAnime.NumeroEpisodios;
        DiretorId = updatedAnime.DiretorId;
        ImageUrl = updatedAnime.ImageUrl;
    }
}