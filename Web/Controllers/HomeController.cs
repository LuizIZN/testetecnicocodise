using Microsoft.AspNetCore.Mvc;

namespace TesteTecnico.Web.Controllers;

public sealed class HomeController : Controller
{
    public IActionResult Index() => View();
}
