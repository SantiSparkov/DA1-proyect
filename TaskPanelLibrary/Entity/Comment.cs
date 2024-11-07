using System.ComponentModel.DataAnnotations;
using TaskPanelLibrary.Entity.Enum;

namespace TaskPanelLibrary.Entity;

public class Comment
{
    public int TaskId { get; set; }
    
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Message is required")]
    public string Message { get; set; }
    
    public User ResolvedBy { get; set; }
    
    public DateTime? ResolvedAt { get; set; }
    
    [Required(ErrorMessage = "Status is required")]
    public EStatusComment Status { get; set; }
    
    
}