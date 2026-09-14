namespace TesteTecnico.Models;

public class Diretor
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = string.Empty;
    public string DataNascimento { get; private set; } = string.Empty;

    public List<Anime> Animes { get; private set; } = [];
}