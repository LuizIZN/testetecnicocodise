namespace TesteTecnico.Application.Dtos;

public class CreateDiretorRequest
{
    public string Nome { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
}