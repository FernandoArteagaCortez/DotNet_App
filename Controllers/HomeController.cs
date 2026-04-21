using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace SeguridadApp.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Login(string username)
    {
        if (string.IsNullOrEmpty(username)) return RedirectToAction("Index");
        
        // Guardamos el nombre en la sesión
        HttpContext.Session.SetString("UsuarioLogueado", username);
        return RedirectToAction("Dashboard");
    }

    public IActionResult Dashboard() => View();
    public IActionResult Bicicletas() => View();
    public IActionResult Sensible() => View();
    public IActionResult Confidencial() => View();


    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index");
    }
}