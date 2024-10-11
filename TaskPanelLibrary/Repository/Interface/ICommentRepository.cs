using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Repository.Interface;

public interface ICommentRepository
{
    public Comment Add(Comment comment);

    public Comment FindById(int id);

    public Comment Delete(int id);

    public List<Comment> GetAll();

    public Comment Update(Comment comment);
}