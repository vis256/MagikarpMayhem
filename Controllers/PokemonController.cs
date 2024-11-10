using System.Security.Claims;
using MagikarpMayhem.Models;
using MagikarpMayhem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagikarpMayhem.Controllers;

public class PokemonController : Controller
{
    private readonly PokemonService PokemonService;
    public PokemonController(PokemonService pokemonService)
    {
        PokemonService = pokemonService;
    }
    
    // GET
    [Authorize]
    public IActionResult Index()
    {
        var userId = User.Identity.IsAuthenticated ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value) : (int?)null;
        if (userId == null)
        {
            return Unauthorized();
        }
        
        var pokemons = PokemonService.GetUserPokemons(userId.Value);
        return View(pokemons);
    }
    
    // GET /Pokemon/User/5
    [Authorize]
    public IActionResult UserPokemons(int id)
    {
        return View("UserPokemons", id);
    }
    
    // GET /Pokemon/Create
    [Authorize]
    public IActionResult Create()
    {
        return View();
    }

    // POST /Pokemon/Create
    [Authorize]
    [HttpPost]
    public IActionResult Create(Pokemon pokemon)
    {
        if (ModelState.IsValid)
        {
            PokemonService.AddPokemon(pokemon);
            return RedirectToAction("Index");
        }
        return View(pokemon);
    }

    // POST /Pokemon/Delete/5
    [Authorize]
    [HttpPost]
    public IActionResult Delete(int id)
    {
        PokemonService.RemovePokemon(id);
        return RedirectToAction("Index");
    }
}