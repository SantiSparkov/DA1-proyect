using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Repository.Interface;

public interface ICommentRepository
{
    public Comment AddComment(Comment comment);

    public Comment GetCommentById(int id);

    public Comment DeleteComment(int id);

    public List<Comment> GetAllComments();

    public Comment UpdateComment(Comment comment);
}