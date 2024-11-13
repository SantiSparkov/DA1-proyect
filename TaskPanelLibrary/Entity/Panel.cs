using System.ComponentModel.DataAnnotations;

namespace TaskPanelLibrary.Entity;

public class Panel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "The name is required")]
    public string Name { get; set; }
    
    [Required(ErrorMessage = "The team is required")]
    public Team Team { get; set; }
    
    [Required(ErrorMessage = "The description is required")]
    public string Description { get; set; }
    
    public List<Task> Tasks { get; set; }
    
    public bool IsDeleted { get; set; } = false;
}
