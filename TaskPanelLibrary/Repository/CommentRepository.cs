using TaskPanelLibrary.Entity;
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
            if (comment.CommentId == id)
            {
                return comment;
            }
        }

        throw new ArgumentException($"Panel with id: {id} does not exist");
    }

    public Comment delete(int id)
    {
        foreach (var comment in _comments)
        {
            if (comment.CommentId == id)
            {
                _comments.Remove(comment);
                return comment;
            }
        }

        throw new ArgumentException($"Comment with id: {id} does not exist");
    }

    public List<Comment> getAll()
    {
        return _comments;
    }

    public Comment update(Comment comment)
    {
        Comment commentSaved = finById(comment.CommentId);
        commentSaved.Status = comment.Status;
        commentSaved.Message = comment.Message;
        commentSaved.ResolvedBy = comment.ResolvedBy;
        commentSaved.ResolvedAt = comment.ResolvedAt;
        return comment;
    }
}