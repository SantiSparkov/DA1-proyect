using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Exception.Comment;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelLibrary.Repository;

public class CommentRepository : ICommentRepository
{
    private readonly List<Comment> _comments;

    public CommentRepository()
    {
        _comments = new List<Comment>();
    }

    public Comment AddComment(Comment comment)
    {
        comment.Id = _comments.Count > 0 ? _comments.Max(t => t.Id) + 1 : 1;
        _comments.Add(comment);
        return comment;
    }

    public Comment GetCommentById(int id)
    {
        List<Comment> comments = GetAllComments();
        foreach (var comment in comments)
        {
            if (comment.Id == id)
            {
                return comment;
            }
        }

        throw new CommentNotValidException($"Panel with id: {id} does not exist");
    }

    public Comment DeleteComment(int id)
    {
        foreach (var comment in _comments)
        {
            if (comment.Id == id)
            {
                _comments.Remove(comment);
                return comment;
            }
        }

        throw new CommentNotValidException($"Comment with id: {id} does not exist");
    }

    public List<Comment> GetAllComments()
    {
        return _comments;
    }

    public Comment UpdateComment(Comment comment)
    {
        Comment commentSaved = GetCommentById(comment.Id);
        commentSaved.Status = comment.Status;
        commentSaved.Message = comment.Message;
        commentSaved.ResolvedBy = comment.ResolvedBy;
        commentSaved.ResolvedAt = comment.ResolvedAt;
        return comment;
    }
}