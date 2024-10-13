using System.ComponentModel.DataAnnotations;

namespace TaskPanelLibrary.Entity;

public class User
{
    public int Id { get; set; }
    
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Email must not be empty.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [RegularExpression(@"^[^@]+@[^@]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email must contain both '@' and '.'")]
    public string Email { get; set; }
    
    public string Password { get; set; }
    
    public string LastName { get; set; }
    
    public DateTime BirthDate { get; set; }
    
    public bool IsAdmin { get; set; }
    
    public Trash Trash { get; set; }
    
    public User()
    {
        Trash = new Trash();
    }
}