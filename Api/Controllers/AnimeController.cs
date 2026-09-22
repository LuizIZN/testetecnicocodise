namespace TesteTecnico.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Anime;

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

    [HttpGet]
    public async Task<ActionResult<QueryResponse<GetAnimeResponse>>> GetAll([FromQuery] QueryAnimeParameters queryParams)
    {
        try
        {
            var animes = await _animeService.GetAllAsync(queryParams);

            return Ok(animes);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<string>> Update(Guid id, UpdateAnimeRequest request)
    {
        try
        {
            _ = await _animeService.UpdateAsync(request, id);
            return CreatedAtAction(nameof(GetById), new { id }, "Anime atualizado com sucesso!");
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("encontrado"))
            {
                return NotFound(ex.Message);
            }

            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            _ = await _animeService.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}