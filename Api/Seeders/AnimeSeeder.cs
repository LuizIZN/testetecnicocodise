using Microsoft.EntityFrameworkCore;
using TesteTecnico.Domain.Models;
using TesteTecnico.Infraestructure.Data;

namespace TesteTecnico.Api.Seeders;

public class AnimeSeeder(AppDbContext context)
{
    private readonly AppDbContext _context = context;

    public async Task SeedAsync()
    {
        if (!_context.Animes.Any())
        {
            var diretores = await _context.Diretores.ToListAsync();
            var animes = new List<Anime>
            {
                new(Nome: "My Neighbor Totoro", Descricao: "Two girls move to the country and discover magical creatures in the forest.", AnoLancamento: 1988, DiretorId: diretores[0].Id, NumeroEpisodios: 1),
                new(Nome: "Your Name", Descricao: "Two teenagers share a profound connection after swapping bodies.", AnoLancamento: 2016, DiretorId: diretores[1].Id, NumeroEpisodios: 1),
                new(Nome: "Wolf Children", Descricao: "A woman raises her half-human, half-wolf children after their father dies.", AnoLancamento: 2012, DiretorId: diretores[2].Id, NumeroEpisodios: 1),
                new(Nome: "Paprika", Descricao: "A psychologist uses a device that allows therapists to help patients by entering their dreams.", AnoLancamento: 2006, DiretorId: diretores[3].Id, NumeroEpisodios: 1),
                new(Nome: "Grave of the Fireflies", Descricao: "A tragic story of two siblings struggling to survive in Japan during World War II.", AnoLancamento: 1988, DiretorId: diretores[4].Id, NumeroEpisodios: 1),
                new(Nome: "Mobile Suit Gundam", Descricao: "In a future where humanity has colonized space, a young pilot becomes involved in a war between Earth and its colonies.", AnoLancamento: 1979, DiretorId: diretores[5].Id, NumeroEpisodios: 43),
                new(Nome: "Cowboy Bebop", Descricao: "In the year 2071, a ragtag crew of bounty hunters travel in their spaceship called the Bebop.", AnoLancamento: 1998, DiretorId: diretores[6].Id, NumeroEpisodios: 26),
                new(Nome: "Neon Genesis Evangelion", Descricao: "Teenagers pilot giant mechs to protect Earth from mysterious beings known as Angels.", AnoLancamento: 1995, DiretorId: diretores[7].Id, NumeroEpisodios: 26),
                new(Nome: "Akira", Descricao: "In a dystopian future, a biker gang member gains psychic powers and threatens to destroy Neo-Tokyo.", AnoLancamento: 1988, DiretorId: diretores[8].Id, NumeroEpisodios: 1),
                new(Nome: "Ghost in the Shell", Descricao: "In a future where cybernetic enhancements are common, a cyborg policewoman hunts a mysterious hacker known as the Puppet Master.", AnoLancamento: 1995, DiretorId: diretores[9].Id, NumeroEpisodios: 1)
            };
            _context.Animes.AddRange(animes);
            await _context.SaveChangesAsync();
        }
    }
}