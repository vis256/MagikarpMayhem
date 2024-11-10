using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using MagikarpMayhem.Attributes;
using MagikarpMayhem.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MagikarpMayhem.Services;
using Microsoft.AspNetCore.Authorization;

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
    
    // GET /User/Index
    [Authorize(Roles = "Professor")]
    public IActionResult Index()
    {
        var users = _context.User.ToList();
        return View(users);
    }
    
    // GET /User/Profile
    [Authorize]
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
    [NotLoggedIn]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [NotLoggedIn]
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
    [NotLoggedIn]
    public async Task<IActionResult> Register(MagikarpMayhem.Models.RegisterDTO userData)
    {
        if (ModelState.IsValid)
        {
            if (!await _authService.Register(userData))
            {
                return View(userData);
            }
            return RedirectToAction(nameof(Login));
        }
        return View(userData);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await _authService.Logout(HttpContext);
        return RedirectToAction("Index", "Home");
    }

    // GET /User/Register
    [NotLoggedIn]
    public IActionResult Register()
    {
        return View();
    }
    
    
    // GET User/NotLoggedIn
    [NotLoggedIn]
    public IActionResult NotLoggedIn()
    {
        return View();
    }
    
    // GET User/AccessDenied
    [AllowAnonymous]
    public IActionResult AccessDenied()
    {
        return View();
    }
    
    // GET /User/Edit/{id}
    [Authorize(Roles = "Professor")]
    public IActionResult Edit(int id)
    {
        var user = _context.User.Find(id);
        if (user == null)
        {
            return NotFound();
        }
        return View(user);
    }

    // POST /User/Edit/{id}
    [HttpPost]
    [Authorize(Roles = "Professor")]
    public async Task<IActionResult> Edit(int id, User updatedUser)
    {
        if (id != updatedUser.Id)
        {
            return BadRequest();
        }

        try
        {
            var existingUser = _context.User.AsNoTracking().FirstOrDefault(u => u.Id == id);
            if (existingUser != null)
            {
                // Stuff we can't update
                updatedUser.PasswordHash = existingUser.PasswordHash;
                updatedUser.PasswordSalt = existingUser.PasswordSalt;
                
                _context.Update(updatedUser);
                await _context.SaveChangesAsync();
            }
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!_context.User.Any(e => e.Id == id))
            {
                return NotFound();
            }

            throw;
        }
        return RedirectToAction(nameof(Index));
    }
    
    // GET /User/ForgotPassword
    [NotLoggedIn]
    public IActionResult ForgotPassword()
    {
        return View();
    }
    
    // POST /User/ForgotPassword
    [HttpPost]
    [NotLoggedIn]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Username == model.Username);        if (user == null)
        {
            ModelState.AddModelError(string.Empty, "Username not found.");
            return View();
        }

        // Generate password reset token and send email (implementation depends on your email service)
        await _authService.SendPasswordResetEmail(user);

        return RedirectToAction(nameof(Login));
    }
    
    // GET /User/ResetPassword/{userId}/{token}
    [NotLoggedIn]
    public async Task<IActionResult> ResetPassword(int userId, string token)
    {
        if (!await _authService.ValidatePasswordResetToken(userId, token))
        {
            return RedirectToAction("NotLoggedIn");
        }
        
        return View();
    }
    
    // POST /User/ResetPassword/{userId}/{token}
    [HttpPost]
    [NotLoggedIn]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)    {
        if (!await _authService.ValidatePasswordResetToken(model.UserId, model.Token))        {
            return RedirectToAction("NotLoggedIn");
        }
        
        if (model.Password != model.ConfirmPassword)        {
            ModelState.AddModelError(string.Empty, "Passwords do not match.");
            return View();
        }
        
        await _authService.ResetPassword(model.UserId, model.Token, model.Password);        
        return RedirectToAction(nameof(Login));
    }
}