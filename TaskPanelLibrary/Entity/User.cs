using System.ComponentModel.DataAnnotations;

namespace TaskPanelLibrary.Entity;

public class User
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name must not be empty.")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "Email must not be empty.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [RegularExpression(@"^[^@]+@[^@]+\.[a-zA-Z]{2,}$", ErrorMessage = "Email must contain both '@' and '.'")]
    public string Email { get; set; }
    
    [Required(ErrorMessage = "Password must not be empty.")]
    public string Password { get; set; }
    
    [Required(ErrorMessage = "First name must not be empty.")]
    public string LastName { get; set; }
    
    public DateTime BirthDate { get; set; }
    
    public bool IsAdmin { get; set; }
    
    public int TrashId { get; set; }
    
    public ICollection<Team> Teams { get; set; }
}