using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagikarpMayhem.Controllers;

public class PokedexController : Controller
{
    private readonly Data.MagikarpMayhemContext _context;

    public PokedexController(Data.MagikarpMayhemContext context)
    {
        _context = context;
    }
    // GET
    [Authorize]
    public IActionResult Index()
    {
        var pokedexEntries = _context.PokedexInfo.ToList();
        return View(pokedexEntries);
    }
}