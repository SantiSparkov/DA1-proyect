using TaskPanelLibrary.Entity;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service.Interface;

public interface ICommentService
{
    public Comment CreateComment(Comment comment);
    
    public Comment GetCommentById(int id);

    public Comment UpdateComment(Comment comment);

    public List<Comment> GetCommentForTask(int panelId);
}