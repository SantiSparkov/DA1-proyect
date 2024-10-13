using TaskPanelLibrary.Entity.Enum;

namespace TaskPanelLibrary.Entity;

public class Comment
{
    public int TaskId { get; set; }
    
    public int Id { get; set; }
    
    public string Message { get; set; }
    
    public User ResolvedBy { get; set; }
    
    public DateTime ResolvedAt { get; set; }
    
    public EStatusComment Status { get; set; }
    
    
}