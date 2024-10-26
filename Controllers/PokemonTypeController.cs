using Microsoft.AspNetCore.Mvc;

namespace MagikarpMayhem.Controllers;

public class PokemonTypeController(Data.MagikarpMayhemContext context) : Controller
{
    private readonly Data.MagikarpMayhemContext _context = context;

    // GET
    public IActionResult Index()
    {
        return View();
    }
    
    // GET /pokemonType/details/5
    public IActionResult Details(int id)
    {
        var pokemonType = _context.PokemonType.Find(id);
        return View();
    }
    
    // GET /pokemonType/create
    public IActionResult Create()
    {
        return View();
    }
    
    // POST /pokemonType/create
    [HttpPost]
    public IActionResult Create(Models.PokemonType pokemonType)
    {
        _context.PokemonType.Add(pokemonType);
        _context.SaveChanges();
        return RedirectToAction("Index");
    }
}