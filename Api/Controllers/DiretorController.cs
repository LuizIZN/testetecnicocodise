using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.Common;
using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Diretor;
using TesteTecnico.Application.Interfaces;

namespace TesteTecnico.Api.Controllers;

[ApiController]
[Route("diretor")]
public sealed class DiretorController(IDiretorService diretorService) : ControllerBase
{
    private readonly IDiretorService _diretorService = diretorService;

    [HttpPost]
    public async Task<ActionResult<Guid>> Create(CreateDiretorRequest request)
    {
        try
        {
            var result = await _diretorService.AddAsync(request);

            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result.Id);
        }
        catch (Error err)
        {
            return err.MapErrorMessage<Guid>();
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<GetDiretorResponse>> GetById(Guid id)
    {
        try
        {
            var diretor = await _diretorService.GetByIdAsync(id);
            return Ok(diretor);
        }
        catch (Error err)
        {
            return err.MapErrorMessage<GetDiretorResponse>();
        }
    }

    [HttpGet]
    public async Task<ActionResult<QueryResponse<GetDiretorResponse>>> GetAll([FromQuery] QueryDiretorParams queryParams)
    {
        try
        {
            var diretores = await _diretorService.GetAllAsync(queryParams);

            return Ok(diretores);
        }
        catch (Error err)
        {
            return err.MapErrorMessage<QueryResponse<GetDiretorResponse>>();
        }
    }
    
    [HttpPatch("{id}")]
    public async Task<ActionResult<string>> Update(Guid id, UpdateDiretorRequest request)
    {
        try
        {
            await _diretorService.UpdateAsync(request, id);

            return CreatedAtAction(nameof(GetById), new { id }, "Diretor atualizado com sucesso!");
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
            await _diretorService.DeleteAsync(id);

            return NoContent();
        }
        catch (Error err)
        {
            return err.MapErrorMessage<string>();
        }
    }

    [HttpGet("{id}/animes")]
    public async Task<ActionResult<GetAnimeDiretorResponse>> GetDiretorWithAnimes(Guid id)
    {
        try
        {
            var diretorWithAnimes = await _diretorService.GetDiretorWithAnimesAsync(id);
            return Ok(diretorWithAnimes);
        }
        catch (Error err)
        {
            return err.MapErrorMessage<GetAnimeDiretorResponse>();
        }
    }
}