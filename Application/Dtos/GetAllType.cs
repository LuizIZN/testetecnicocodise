namespace TesteTecnico.Application.Dtos;

public class GetAllType<T>
{
    public IEnumerable<T> Items { get; set; } = [];
    public int TotalCount { get; set; }
}