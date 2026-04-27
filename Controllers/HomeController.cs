using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient; // Asegúrate de tener este using

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

    [HttpGet("vulnerable/file")]
    public IActionResult GetVulnerableFile(string name) // <-- "Taint Source" (Entrada sucia)
    {
        // El IAST de Datadog rastrea este parámetro 'name'
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", name);
        
        // Aquí es donde IAST detecta la vulnerabilidad ("Sink")
        // Porque 'path' contiene datos del usuario sin validar.
        var content = System.IO.File.ReadAllText(path); 
        
        return Ok(content);
    }

[HttpGet("vulnerable/user")]
public IActionResult GetUser(string id) // <-- Source (Entrada sucia)
{
    // VULNERABILIDAD: Concatenación directa de strings para SQL
    // El IAST rastreará 'id' hasta que llegue al comando Execute
    string query = "SELECT * FROM Users WHERE id = '" + id + "'";
    
    try 
    {
        // Simulamos la ejecución. IAST detectará el "Sink" aquí 
        // incluso si la conexión falla o no existe.
        using (var connection = new SqlConnection("Server=fake;Database=fake;User Id=fake;Password=fake;"))
        {
            using (var command = new SqlCommand(query, connection))
            {
                // Datadog Code Security intercepta este punto
                Console.WriteLine("Ejecutando query: " + query);
                // command.ExecuteReader(); 
            }
        }
    }
    catch (Exception) { /* Ignoramos el error de conexión real */ }

    return Ok("Consulta enviada al motor de análisis: " + query);
}
    

    
}