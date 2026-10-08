using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.Interfaces;
using TesteTecnico.Application.Dtos.Diretor;

namespace TesteTecnico.Api.Controllers;

public sealed class DiretorViewController(IDiretorService diretorService) : Controller
{
    private readonly IDiretorService _diretorService = diretorService;

    [HttpGet("/diretores")]
    public async Task<IActionResult> Index(
        [FromQuery] QueryDiretorParams queryParams)
    {
        var diretores = await _diretorService.GetAllAsync(queryParams);

        return View(diretores);
    }
}