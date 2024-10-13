using System.ComponentModel.DataAnnotations;

namespace TaskPanelLibrary.Entity;

public class Login
{
    [Required(ErrorMessage = "Email must not be empty.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [RegularExpression(@"^[^@]+@[^@]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email must contain both '@' and '.'")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password must not be empty.")]
    public string Password { get; set; }
}