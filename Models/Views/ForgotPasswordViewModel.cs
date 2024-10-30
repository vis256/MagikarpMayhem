using Microsoft.Build.Framework;

namespace MagikarpMayhem.Models;

public class ForgotPasswordViewModel
{
    [Required] public string Username { get; set; }
}