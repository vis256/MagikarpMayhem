using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MagikarpMayhem.Models;

[Index(nameof(Username), IsUnique = true)]
public class User
{
    [Key]
    public int Id { get; set; }

    [Microsoft.Build.Framework.Required]
    public string DisplayName { get; set; }
    
    [Microsoft.Build.Framework.Required]
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string PasswordSalt { get; set; }
    public UserRole Role { get; set; }
}