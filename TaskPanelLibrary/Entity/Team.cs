using System.ComponentModel.DataAnnotations;

namespace TaskPanelLibrary.Entity;

public class Team
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Name must not be empty.")]
    public string Name { get; set; }
    
    public DateTime CreationDate { get; set; }
    
    [Required(ErrorMessage = "Description must not be empty.")]
    public string TasksDescription { get; set; }
    
    [Required(ErrorMessage = "Max amount of members must not be empty.")]
    [Range(1, 100, ErrorMessage = "Max amount of members must be between 1 and 100.")]
    public int MaxAmountOfMembers { get; set; }
    
    public List<User> Users { get; set; }
    
    public List<Panel> Panels { get; set; }
    
    public int TeamLeaderId { get; set; }
    
    public Team()
    {
        Users = new List<User>();
        Panels = new List<Panel>();
    }
    
}