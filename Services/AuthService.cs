using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using MagikarpMayhem.Data;
using MagikarpMayhem.Models;

namespace MagikarpMayhem.Services;

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

public class AuthService
{
    private readonly MagikarpMayhemContext _context;

    public AuthService(MagikarpMayhemContext context)
    {
        _context = context;
    }

    public async Task<bool> Login(HttpContext httpContext, string username, string password)
    {
        var user = await _context.User.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null || user.PasswordHash != PasswordHelper.HashPassword(password, Convert.FromBase64String(user.PasswordSalt)))
        {
            return false;
        }

        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
            new(ClaimTypes.Role, user.Role.ToString())
        };

        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var authProperties = new AuthenticationProperties
        {
            IsPersistent = true,
            ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
            RedirectUri = "/User/Profile"
        };

        await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
        return true;
    }

    public async Task Register(LoginDTO userData)
    {
        var salt = PasswordHelper.GenerateSalt();
        var user = new User
        {
            Username = userData.Username,
            PasswordHash = PasswordHelper.HashPassword(userData.Password, salt),
            PasswordSalt = Convert.ToBase64String(salt),
            Role = UserRole.User
        };
        _context.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task Logout(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }
}