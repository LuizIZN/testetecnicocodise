using Microsoft.AspNetCore.Mvc;

namespace TesteTecnico.Api.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
