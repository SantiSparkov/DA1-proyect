using System.ComponentModel.DataAnnotations;
using TaskPanelLibrary.Entity.Enum;

namespace TaskPanelLibrary.Entity;

public class Task
{
    public List<Comment> CommentList { get; set; }

    public int Id { get; set; }

    public int PanelId { get; set;}

    [Required(ErrorMessage = "The title is required")]
    public string Title { get; set; }
    
    [Required(ErrorMessage = "Description is required")]
    public string Description { get; set; }
    
    [Required(ErrorMessage = "Due date is required")]
    public DateTime DueDate { get; set; }
    
    [Required(ErrorMessage = "Priority is required")]
    public EPriority Priority { get; set; }
    
    public bool IsDeleted { get; set; } = false;
    
    public int? EpicId { get; set; }
    
    public Task()
    {
        CommentList = new List<Comment>();
    }
    
}