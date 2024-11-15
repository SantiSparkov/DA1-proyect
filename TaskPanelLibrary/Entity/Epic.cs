using System.ComponentModel.DataAnnotations;
using TaskPanelLibrary.Entity.Enum;
namespace TaskPanelLibrary.Entity;

public class Epic
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "The title is required")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Priority is required")]
    public EPriority Priority { get; set; }
    
    [Required(ErrorMessage = "The description is required")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Due date is required")]
    public DateTime DueDateTime { get; set; }
    
    public int PanelId { get; set; }
    
    public List<Task> Tasks { get; set; }
    
    public Epic()
    {
        Tasks = new List<Task>();
    }
}
