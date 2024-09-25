namespace TaskPanelLibrary.Entity;

public class Comment
{
    public int CommentId { get; set; }
    
    public string Message { get; set; }
    
    public User ResolvedBy { get; set; }
    
    public DateTime ResolvedAt { get; set; }
    
    public EStatus Status { get; set; }
    
    public enum EStatus
    {
        RESOLVED,
        PENDING
    }
}