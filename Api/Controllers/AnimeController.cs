namespace TesteTecnico.Application.Controllers;
using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Dtos;

[ApiController]
[Route("anime")]
public sealed class AnimeController(IAnimeService animeService) : ControllerBase
{
    private readonly IAnimeService _animeService = animeService;

    [HttpPost]
    public ActionResult<string> Create(CreateAnimeRequest request)
    {
        try
        {
            var result = _animeService.AddAsync(request);

            return CreatedAtAction(nameof(result.Id), new { id = result.Id }, "Anime criado com sucesso.");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    } 
}