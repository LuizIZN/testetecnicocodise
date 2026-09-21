namespace TesteTecnico.Application.Dtos;
public sealed class GetAnimeDiretorResponse
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public List<GetAnimeLookup> Animes { get; set; } = [];
}