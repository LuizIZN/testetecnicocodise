using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Dtos.Anime;
using TesteTecnico.Application.Dtos.Diretor;
using TesteTecnico.Api.ViewModels;

namespace TesteTecnico.Api.Controllers;

public sealed class AnimeViewController(
    IAnimeService animeService,
    IDiretorService diretorService) : Controller
{
    private readonly IAnimeService _animeService = animeService;
    private readonly IDiretorService _diretorService = diretorService;

    [HttpGet("/animes")]
    public async Task<IActionResult> Index(
        [FromQuery] QueryAnimeParameters queryParams)
    {
        var animes = await _animeService.GetAllAsync(queryParams);
        var diretores = await _diretorService.GetAllAsync(new QueryDiretorParams());

        return View(new AnimeViewModel
        {
            Animes = animes,
            Diretores = diretores
        });
    }
}