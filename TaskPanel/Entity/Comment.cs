namespace TaskPanel.Models.Entity;

public class Comment
{
    public string Message { get; set; }
    
    public User User { get; set; }
    
    public DateTime CreateDate { get; set; }
    
    public EStatus Status { get; set; }
    
    public enum EStatus
    {
        Active,
        Passive
    }
}