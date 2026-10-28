using Microsoft.AspNetCore.Mvc;
using TesteTecnico.Application.Dtos;
using TesteTecnico.Application.Dtos.Anime;
using TesteTecnico.Application.Dtos.Diretor;
using TesteTecnico.Web.Clients;
using TesteTecnico.Web.Helpers;
using TesteTecnico.Web.ViewModels;

namespace TesteTecnico.Web.Controllers;

public sealed class AnimeViewController(IAnimeApiClient animeApiClient, IDiretorApiClient diretorApiClient) : Controller
{
    [HttpGet("/animes")]
    public async Task<IActionResult> Index([FromQuery] QueryAnimeParameters parameters, CancellationToken cancellationToken)
    {
        try
        {
            var animes = await animeApiClient.GetAllAsync(parameters, cancellationToken);

            var diretores = await diretorApiClient.GetAllAsync(new QueryDiretorParams(), cancellationToken);
            return View(new AnimeViewModel { Animes = animes, Diretores = diretores });
        }
        catch (HttpRequestException exception)
        {
            TempData["ErrorMessage"] = ApiErrorMessage.FromExceptionMessage(exception.Message);
            return RedirectToAction(nameof(Index));
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return StatusCode(
                StatusCodes.Status504GatewayTimeout,
                "A API demorou para responder. Tente novamente.");
        }
    }

    [HttpPost("/animes")]
    public async Task<IActionResult> Create(CreateAnimeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await animeApiClient.CreateAsync(request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Anime criado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return await ApiError(response);
        }
        catch (HttpRequestException exception)
        {
            return RedirectWithError(exception);
        }
    }

    [HttpPost("/animes/{id:guid}/editar")]
    public async Task<IActionResult> Update(Guid id, UpdateAnimeRequest request, CancellationToken cancellationToken)
    {
        try
        {
            using var response = await animeApiClient.UpdateAsync(id, request, cancellationToken);
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Anime atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            return await ApiError(response);
        }
        catch (HttpRequestException exception)
        {
            return RedirectWithError(exception);
        }
    }

    [HttpPost("/animes/{id:guid}/excluir")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        using var response = await animeApiClient.DeleteAsync(id, cancellationToken);
        if (response.IsSuccessStatusCode) 
        {
            TempData["SuccessMessage"] = "Anime excluído com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        return await ApiError(response);
    }

    private async Task<IActionResult> ApiError(HttpResponseMessage response)
    {
        TempData["ErrorMessage"] = await ApiErrorMessage.FromResponseAsync(response);
        return RedirectToAction(nameof(Index));
    }

    private IActionResult RedirectWithError(HttpRequestException exception)
    {
        TempData["ErrorMessage"] = ApiErrorMessage.FromExceptionMessage(exception.Message);
        return RedirectToAction(nameof(Index));
    }
}
