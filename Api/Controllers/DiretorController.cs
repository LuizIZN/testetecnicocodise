using Microsoft.AspNetCore.Mvc;
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
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
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
        catch (Exception ex)
        {
            return NotFound(ex.Message);
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
        catch (Exception ex)
        {
            return NotFound(ex.Message);
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
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<string>> Delete(Guid id)
    {
        try
        {
            await _diretorService.DeleteAsync(id);

            return Ok("Diretor excluído com sucesso!");
        }
        catch (Exception ex)
        {
            if (ex.Message.Contains("não encontrado"))
            {
                return NotFound(ex.Message);
            }
            return BadRequest("O diretor está associado a um anime e não pode ser excluído.");
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
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
}