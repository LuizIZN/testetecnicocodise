using Microsoft.AspNetCore.Mvc;

namespace TesteTecnico.Application.Common;

public class Error(int code, string message, List<string>? errors) : Exception(message)
{
    public int Code { get; private set; } = code;
    public List<string>? Errors { get; private set; } = errors;

    public ActionResult<T> MapErrorMessage<T>()
    {
        return Code switch
        {
            400 => new BadRequestObjectResult(Errors),
            404 => new NotFoundObjectResult(Message),
            500 => new ObjectResult("Erro interno.")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            },
            _ => new ObjectResult("Erro inesperado aconteceu.")
            {
                StatusCode = StatusCodes.Status500InternalServerError
            }
        };
    }
}