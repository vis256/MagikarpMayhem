using Microsoft.AspNetCore.Mvc;

namespace MagikarpMayhem.Controllers;

public class UserController : Controller
{
    private readonly Data.MagikarpMayhemContext _context;

    public UserController(Data.MagikarpMayhemContext context)
    {
        _context = context;
    }
    
    // GET /user
    public IActionResult Index()
    {
        var users = _context.User.ToList();
        return View(users);
    }
}