namespace TesteTecnico.Api.Controllers;
using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Anime;
using TesteTecnico.Api.Common;
using FluentValidation;

[ApiController]
[Route("api/anime")]
public sealed class AnimeController(
    IAnimeService animeService,
    IImageStorage imageStorage) : Controller
{
    private readonly IAnimeService _animeService = animeService;
    private readonly IImageStorage _imageStorage = imageStorage;

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(
        [FromForm] CreateAnimeRequest request,
        IValidator<CreateAnimeRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            var errors = validation.ToDictionary();
            return BadRequest(new ValidationProblemDetails(errors));
        }

        try
        {
            if (request.Imagem is not null)
            {
                request.ImageUrl = await _imageStorage.SaveAsync(
                    request.Imagem,
                    "uploads/animes",
                    HttpContext.RequestAborted);
            }

            var result = await _animeService.AddAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result.Id);
        }
        catch (InvalidOperationException err)
        {
            return BadRequest(new { title = err.Message });
        }
        catch (Error err)
        {
            await _imageStorage.DeleteAsync(request.ImageUrl, HttpContext.RequestAborted);
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
    public async Task<ActionResult<QueryResponse<GetAnimeResponse>>> GetAll(
        [FromQuery] QueryAnimeParameters queryParams,
        IValidator<QueryAnimeParameters> validator)
    {
        var validation = await validator.ValidateAsync(queryParams);

        if (!validation.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(validation.ToDictionary()));
        }

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
    public async Task<ActionResult<string>> Update(
        Guid id,
        [FromForm] UpdateAnimeRequest request,
        IValidator<UpdateAnimeRequest> validator)
    {
        var validation = await validator.ValidateAsync(request);

        if (!validation.IsValid)
        {
            return BadRequest(new ValidationProblemDetails(validation.ToDictionary()));
        }

        try
        {
            if (request.Imagem is not null)
            {
                var existingAnime = await _animeService.GetByIdAsync(id)
                    ?? throw new Error(404, "Anime não encontrado.", []);

                request.ImageUrl = await _imageStorage.UpdateAsync(
                    existingAnime.ImageUrl,
                    request.Imagem,
                    "uploads/animes",
                    HttpContext.RequestAborted);
            }
            await _animeService.UpdateAsync(request, id);
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
            var existingAnime = await _animeService.GetByIdAsync(id)
                ?? throw new Error(404, "Anime não encontrado.", []);

            if (!string.IsNullOrEmpty(existingAnime.ImageUrl))
            {
                await _imageStorage.DeleteAsync(existingAnime.ImageUrl, HttpContext.RequestAborted);
            }

            await _animeService.DeleteAsync(id);

            return NoContent();
        }
        catch (Error err)
        {
            return err.MapErrorMessage<string>();
        }
    }
}