using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelLibrary.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly List<Comment> _comments;

    public CommentRepository()
    {
        _comments = new List<Comment>();
    }

    public Comment Add(Comment comment)
    {
        _comments.Add(comment);
        return comment;
    }

    public Comment FindById(int id)
    {
        List<Comment> comments = GetAll();
        foreach (var comment in comments)
        {
            if (comment.Id == id)
            {
                return comment;
            }
        }

        throw new TaskPanelException($"Panel with id: {id} does not exist");
    }

    public Comment Delete(int id)
    {
        foreach (var comment in _comments)
        {
            if (comment.Id == id)
            {
                _comments.Remove(comment);
                return comment;
            }
        }

        throw new TaskPanelException($"Comment with id: {id} does not exist");
    }

    public List<Comment> GetAll()
    {
        return _comments;
    }

    public Comment Update(Comment comment)
    {
        Comment commentSaved = FindById(comment.Id);
        commentSaved.Status = comment.Status;
        commentSaved.Message = comment.Message;
        commentSaved.ResolvedBy = comment.ResolvedBy;
        commentSaved.ResolvedAt = comment.ResolvedAt;
        return comment;
    }
}