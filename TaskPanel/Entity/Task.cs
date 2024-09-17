namespace TaskPanel.Models.Entity;

public class Task
{
    public List<Comment> CommentList { get; set; }
    
    public Task()
    {
        CommentList = new List<Comment>();
    }
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public DateTime DueDate { get; set; }
    
    public TaskPriority Priority { get; set; }
    
    public enum TaskPriority
    {
        High,
        Medium,
        Low
    }
}