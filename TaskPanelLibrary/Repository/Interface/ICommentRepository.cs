using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Repository.Interface;

public interface ICommentRepository
{
    public Comment add(Comment comment);

    public Comment finById(int id);

    public Comment delete(int id);

    public List<Comment> getAll();

    public Comment update(Comment comment);
}