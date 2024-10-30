using System.ComponentModel.DataAnnotations;

namespace MagikarpMayhem.Models;

public class ResetPasswordViewModel
{
    [Required]
    public int UserId { get; set; }

    [Required]
    public string Token { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; set; }
}