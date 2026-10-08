using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Dtos.Anime;

namespace TesteTecnico.Api.Controllers;

public sealed class AnimeViewController(IAnimeService animeService) : Controller
{
    private readonly IAnimeService _animeService = animeService;

    [HttpGet("/animes")]
    public async Task<IActionResult> Index(
        [FromQuery] QueryAnimeParameters queryParams)
    {
        var animes = await _animeService.GetAllAsync(queryParams);

        return View(animes);
    }
}