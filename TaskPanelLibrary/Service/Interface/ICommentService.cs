using TaskPanelLibrary.Entity;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service.Interface;

public interface ICommentService
{
    public Comment CreateComment();
    
    public Comment FindById(int id);

    public Comment DeleteComment(Task task, Comment comment);

    public Comment UpdateComment(Comment comment);

    public Comment AddComment(Comment comment);

    public List<Comment> GetCommentForTask(int panelId);


}