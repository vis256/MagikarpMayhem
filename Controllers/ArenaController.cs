using System.Security.Claims;
using MagikarpMayhem.Attributes;
using MagikarpMayhem.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MagikarpMayhem.Controllers;

public class ArenaController : Controller
{
    private readonly ArenaService ArenaService;

    public ArenaController(ArenaService arenaService)
    {
        ArenaService = arenaService;
    }

    // GET
    [Authorize]
    public IActionResult Index()
    {
        return View();
    }

    // GET /Arena/View/5
    [Authorize]
    public IActionResult View(int id)
    {
        // Check if user is member of the arena
        var userId = User.Identity.IsAuthenticated
            ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
            : (int?)null;
        if (userId == null)
        {
            return Unauthorized();
        }

        var arena = ArenaService.GetArenaById(id).Result;
        if (arena == null)
        {
            return NotFound();
        }

        var isMember = ArenaService.IsMemberOfArena(userId.Value, id).Result;
        if (!isMember)
        {
            return Unauthorized();
        }

        return View(arena);
    }

    // POST /Arena/Join/5
    [HttpPost]
    [Authorize]
    public IActionResult Join(int id)
    {
        // Logic to join the arena
        var userId = User.Identity.IsAuthenticated
            ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
            : (int?)null;
        if (userId == null)
        {
            return Unauthorized();
        }

        var result = ArenaService.JoinArena(userId.Value, id);
        if (result.Result)
        {
            return RedirectToAction("View", new { id });
        }

        return BadRequest("Unable to join the arena");
    }

    // POST /Arena/Leave
    [HttpPost]
    [Authorize]
    public IActionResult Leave(int id)
    {
        // Logic to leave the arena
        var userId = User.Identity.IsAuthenticated
            ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value)
            : (int?)null;
        if (userId == null)
        {
            return Unauthorized();
        }

        var result = ArenaService.LeaveArena(userId.Value, id);
        if (result.Result)
        {
            return RedirectToAction("Index");
        }

        return BadRequest("Unable to leave the arena");
    }
}