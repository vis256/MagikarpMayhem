using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;


namespace MagikarpMayhem.Controllers;


public static class PasswordHelper
{
    public static string HashPassword(string password, byte[] salt)
    {
        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
    }

    public static byte[] GenerateSalt()
    {
        byte[] salt = new byte[128 / 8];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
}

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
        var username = User.Identity?.Name;
        
        if (username == null)
        {
            return RedirectToAction("NotLoggedIn");
        }
        
        var user = _context.User.FirstOrDefault(u => u.Username == username);
        
        if (user == null)
        {
            return RedirectToAction("NotLoggedIn");
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
        var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null || user.PasswordHash != PasswordHelper.HashPassword(password, Convert.FromBase64String(user.PasswordSalt)))
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
    public async Task<IActionResult> Register(MagikarpMayhem.Models.LoginDTO userData)
    {
        if (ModelState.IsValid)
        {
            var salt = PasswordHelper.GenerateSalt();
            var user = new Models.User
            {
                Username = userData.Username,
                PasswordHash = PasswordHelper.HashPassword(userData.Password, salt),
                PasswordSalt = Convert.ToBase64String(salt)
            };
            _context.Add(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Login));
        }
        return View(userData);
    }
    
    // POST /User/Logout
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
    
    // GET User/NotLoggedIn
    public IActionResult NotLoggedIn()
    {
        return View();
    }
}