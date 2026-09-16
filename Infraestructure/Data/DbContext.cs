namespace TesteTecnico.Infraestructure.Data;

using Microsoft.EntityFrameworkCore;

using TesteTecnico.Domain.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Anime> Animes { get; set; }
    public DbSet<Diretor> Diretores { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Anime>()
            .HasOne(a => a.Diretor)
            .WithMany()
            .HasForeignKey(a => a.DiretorId);
    }
}