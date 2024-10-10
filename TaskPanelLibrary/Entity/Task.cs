using TaskPanelLibrary.Entity.Enum;

namespace TaskPanelLibrary.Entity;

public class Task
{
    public List<Comment> CommentList { get; set; }

    public int Id { get; set; }

    public int PanelId { get; set;}
    
    public string Title { get; set; }
    
    public string Description { get; set; }
    
    public DateTime DueDate { get; set; }
    
    public TaskPriority Priority { get; set; }
    
    public Task()
    {
        CommentList = new List<Comment>();
    }

    public void AddComment(Comment comment)
    {
        CommentList.Add(comment);
    }
    
}