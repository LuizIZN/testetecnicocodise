namespace TesteTecnico.Application.Dtos;
public sealed class GetDiretorResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
}