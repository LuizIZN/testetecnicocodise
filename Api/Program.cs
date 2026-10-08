using Microsoft.EntityFrameworkCore;
using TesteTecnico.Api.Seeders;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Services;
using TesteTecnico.Infraestructure.Data;
using TesteTecnico.Infraestructure.Repositories;
using Scalar.AspNetCore;
using FluentValidation;
using TesteTecnico.Application.Validators;
using ColorlibHQ.AdminLTE.AspNetCore;
using ColorlibHQ.AdminLTE.AspNetCore.Menu;

var builder = WebApplication.CreateBuilder();

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? throw new InvalidOperationException("DB_CONNECTION_STRING environment variable is not set.");

builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IAnimeService, AnimeService>();
builder.Services.AddScoped<IAnimeRepository, AnimeRepository>();
builder.Services.AddScoped<IDiretorRepository, DiretorRepository>();
builder.Services.AddScoped<IDiretorService, DiretorService>();

builder.Services.AddValidatorsFromAssemblyContaining<CreateAnimeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateAnimeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateDiretorValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateDiretorValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<QueryAnimeValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<QueryDiretorValidator>();

builder.Services.AddAdminLte(options =>
{
    options.BrandText = "Animes API";
    options.SidebarTheme = "dark";
    options.DefaultColorMode = "auto";
    options.Menu = new List<MenuItem>
    {
        new() { Header = "Navegação" },
        new() { Text = "Dashboard", Url = "/", Icon = "bi bi-speedometer" },
        new() { Text = "Animes", Url = "/animes", Icon = "bi bi-film" },
        new() { Text = "Diretores", Url = "/diretores", Icon = "bi bi-person" },
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.MapOpenApi();
    app.MapScalarApiReference();
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

    await context.Database.MigrateAsync();
    
    var animeSeeder = new AnimeSeeder(context);
    var diretorSeeder = new DiretorSeeder(context);

    await diretorSeeder.SeedAsync();
    await animeSeeder.SeedAsync();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
