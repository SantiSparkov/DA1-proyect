using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelLibrary.Repository;

public class CommentRepository : ICommentRepository
{
    private List<Comment> _comments;

    public CommentRepository()
    {
        _comments = new List<Comment>();
    }

    public Comment add(Comment comment)
    {
        _comments.Add(comment);
        return comment;
    }

    public Comment finById(int id)
    {
        List<Comment> comments = getAll();
        foreach (var comment in comments)
        {
            if (comment.Id == id)
            {
                return comment;
            }
        }

        throw new TaskPanelException($"Panel with id: {id} does not exist");
    }

    public Comment delete(int id)
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

    public List<Comment> getAll()
    {
        return _comments;
    }

    public Comment update(Comment comment)
    {
        Comment commentSaved = finById(comment.Id);
        commentSaved.Status = comment.Status;
        commentSaved.Message = comment.Message;
        commentSaved.ResolvedBy = comment.ResolvedBy;
        commentSaved.ResolvedAt = comment.ResolvedAt;
        return comment;
    }
}