using Microsoft.AspNetCore.Mvc;

namespace MagikarpMayhem.Controllers;

public class ArenaController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    // GET /Arena/View/5
    public IActionResult View(int id)
    {
        return View();
    }
}