namespace TesteTecnico.Domain.Models;

public class Diretor(string Nome, DateOnly DataNascimento)
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = Nome;
    public DateOnly DataNascimento { get; private set; } = DataNascimento;
    public List<Anime> Animes { get; private set; } = [];

    public void Update(Diretor updatedDiretor)
    {
        Nome = updatedDiretor.Nome;
        DataNascimento = updatedDiretor.DataNascimento;
    }
}