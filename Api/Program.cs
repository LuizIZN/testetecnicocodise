using Microsoft.EntityFrameworkCore;
using TesteTecnico.Api.Seeders;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Services;
using TesteTecnico.Domain.Models;
using TesteTecnico.Infraestructure.Data;
using TesteTecnico.Infraestructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

builder.Services.AddScoped<IAnimeService, AnimeService>();
builder.Services.AddScoped<IAnimeRepository, AnimeRepository>();
builder.Services.AddScoped<IDiretorRepository, DiretorRepository>();
builder.Services.AddScoped<IDiretorService, DiretorService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.MapGet("/health-check", () =>
{
    return "API funcionando!";
})
.WithName("HealthCheck");

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();
    
    var animeSeeder = new AnimeSeeder(context);
    var diretorSeeder = new DiretorSeeder(context);

    await diretorSeeder.SeedAsync();
    await animeSeeder.SeedAsync();
}

app.Run();
