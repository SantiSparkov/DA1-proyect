using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service;

public class CommentService : ICommentService
{
    public Comment FindById(Panel panel, int id)
    {
        throw new NotImplementedException();
    }

    public Comment DeleteComment(Task task, Comment comment)
    {
        throw new NotImplementedException();
    }

    public Comment UpdateComment(Task task, Comment comment)
    {
        throw new NotImplementedException();
    }

    public Comment AddComment(Task task, Comment comment)
    {
        throw new NotImplementedException();
    }
}