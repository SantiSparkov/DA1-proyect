using TaskPanelLibrary.Entity;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service.Interface;

public interface ICommentService
{
    public Comment FindById(Task task, int id);

    public Comment DeleteComment(Task task, Comment comment);

    public Comment UpdateComment(Task task, Comment comment);
}