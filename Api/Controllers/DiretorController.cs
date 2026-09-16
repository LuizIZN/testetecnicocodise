using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.Dtos;
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
}