using MagikarpMayhem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagikarpMayhem.Controllers;

public class PokedexController : Controller
{
    private readonly PokedexService PokedexService;

    public PokedexController(PokedexService pokedexService)
    {
        PokedexService = pokedexService;
    }
    // GET
    [AllowAnonymous]
    public IActionResult Index()
    {
        var pokedexEntries = PokedexService.GetAllPokedexInfos();
        return View(pokedexEntries);
    }
}