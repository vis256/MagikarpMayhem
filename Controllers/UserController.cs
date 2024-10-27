using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MagikarpMayhem.Services;

namespace MagikarpMayhem.Controllers;

public class UserController : Controller
{
    private readonly Data.MagikarpMayhemContext _context;
    private readonly AuthService _authService;

    public UserController(Data.MagikarpMayhemContext context, AuthService authService)
    {
        _context = context;
        _authService = authService;
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

    [HttpPost]
    public async Task<IActionResult> Login(string username, string password)
    {
        if (!await _authService.Login(HttpContext, username, password))
        {
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View();
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Register(MagikarpMayhem.Models.LoginDTO userData)
    {
        if (ModelState.IsValid)
        {
            await _authService.Register(userData);
            return RedirectToAction(nameof(Login));
        }
        return View(userData);
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await _authService.Logout(HttpContext);
        return RedirectToAction("Index", "Home");
    }

    // GET /User/Register
    public IActionResult Register()
    {
        return View();
    }
    
    
    // GET User/NotLoggedIn
    public IActionResult NotLoggedIn()
    {
        return View();
    }
}