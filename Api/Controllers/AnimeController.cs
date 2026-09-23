namespace TesteTecnico.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Anime;
using TesteTecnico.Application.Common;

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
        catch (Error err)
        {
            return err.MapErrorMessage<Guid>();
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
        catch (Error err)
        {
            return err.MapErrorMessage<GetAnimeResponse>();
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
        catch (Error err)
        {
            return err.MapErrorMessage<QueryResponse<GetAnimeResponse>>();
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
        catch (Error err)
        {
            return err.MapErrorMessage<string>();
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Delete(Guid id)
    {
        try
        {
            await _animeService.DeleteAsync(id);

            return NoContent();
        }
        catch (Error err)
        {
            return err.MapErrorMessage<string>();
        }
    }
}