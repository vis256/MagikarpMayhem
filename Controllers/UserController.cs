using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    
    // GET /User/Profile
    public IActionResult Profile()
    {
        var username = User.Identity.Name;
        var user = _context.User.FirstOrDefault(u => u.Username == username);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }
    
    // GET /User/Login
    public IActionResult Login()
    {
        return View();
    }

    // POST /User/Login
    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username && u.Password == password);
        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.IsAdmin ? "Admin" : "User")
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60)
        };

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

        return RedirectToAction("Index", "Home");
    }

    // GET /User/Register
    public IActionResult Register()
    {
        return View();
    }
    
    // POST /User/Register
    [HttpPost]
    public async Task<IActionResult> Register(MagikarpMayhem.Models.User user)
    {
        if (ModelState.IsValid)
        {
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Login));
        }
        return View(user);
    }

    // POST /User/Logout
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}