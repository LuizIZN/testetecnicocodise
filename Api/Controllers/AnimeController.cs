namespace TesteTecnico.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Dtos;
using TesteTecnico.Domain.Models;

[ApiController]
[Route("anime")]
public sealed class AnimeController(IAnimeService animeService) : ControllerBase
{
    private readonly IAnimeService _animeService = animeService;

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateAnimeRequest request)
    {
        try
        {
            var result = await _animeService.AddAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result.Id);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    } 

    [HttpGet("{id}")]
    public async Task<ActionResult<GetAnimeResponse>> GetById(Guid id)
    {
        try
        {
            var anime = await _animeService.GetByIdAsync(id);
            return Ok(anime);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}