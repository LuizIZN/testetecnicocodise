using TesteTecnico.Domain.Models;
using TesteTecnico.Infraestructure.Data;

namespace TesteTecnico.Api.Seeders;

public class DiretorSeeder(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task SeedAsync()
    {
        if (!_context.Diretores.Any())
        {
            var diretores = new List<Diretor>
            {
                new(Nome: "Hayao Miyazaki", DataNascimento: "1941-01-05"), 
                new(Nome: "Makoto Shinkai", DataNascimento: "1973-02-09"), 
                new(Nome: "Mamoru Hosoda", DataNascimento: "1967-09-19"), 
                new(Nome: "Satoshi Kon", DataNascimento: "1963-10-12"), 
                new(Nome: "Isao Takahata", DataNascimento: "1935-10-29"), 
                new(Nome: "Yoshiyuki Tomino", DataNascimento: "1941-11-05"), 
                new(Nome: "Shinichiro Watanabe", DataNascimento: "1965-05-24"), 
                new(Nome: "Hideaki Anno", DataNascimento: "1960-05-22"), 
                new(Nome: "Katsuhiro Otomo", DataNascimento: "1954-04-14"), 
                new(Nome: "Mamoru Oshii", DataNascimento: "1951-08-08")
            };

            _context.Diretores.AddRange(diretores);
            await _context.SaveChangesAsync();
        }
    }
}