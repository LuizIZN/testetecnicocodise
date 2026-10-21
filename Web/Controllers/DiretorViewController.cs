using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Diretor;
using TesteTecnico.Web.Clients;

namespace TesteTecnico.Web.Controllers;

public sealed class DiretorViewController(IDiretorApiClient diretorApiClient) : Controller
{
    [HttpGet("/diretores")]
    public async Task<IActionResult> Index([FromQuery] QueryDiretorParams parameters, CancellationToken cancellationToken) =>
        View(await diretorApiClient.GetAllAsync(parameters, cancellationToken));

    [HttpPost("/diretores")]
    public async Task<IActionResult> Create(CreateDiretorRequest request, CancellationToken cancellationToken)
    {
        using var response = await diretorApiClient.CreateAsync(request, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Diretor criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        return await ApiError(response);
    }

    [HttpPost("/diretores/{id:guid}/editar")]
    public async Task<IActionResult> Update(Guid id, UpdateDiretorRequest request, CancellationToken cancellationToken)
    {
        using var response = await diretorApiClient.UpdateAsync(id, request, cancellationToken);
        if (response.IsSuccessStatusCode)
        {
            TempData["SuccessMessage"] = "Diretor atualizado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        return await ApiError(response);
    }

    [HttpPost("/diretores/{id:guid}/excluir")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        using var response = await diretorApiClient.DeleteAsync(id, cancellationToken);
        return response.IsSuccessStatusCode
            ? RedirectToAction(nameof(Index))
            : await ApiError(response);
    }

    private async Task<IActionResult> ApiError(HttpResponseMessage response)
    {
        TempData["ErrorMessage"] = await response.Content.ReadAsStringAsync();
        return RedirectToAction(nameof(Index));
    }
}
