namespace TesteTecnico.Domain.Models;

public class Diretor(string Nome, string DataNascimento)
{
    public Guid Id { get; private set; }
    public string Nome { get; private set; } = Nome;
    public string DataNascimento { get; private set; } = DataNascimento;
}