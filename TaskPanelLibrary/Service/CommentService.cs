using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service;

public class CommentService : ICommentService
{
    private CommentRepository _commentRepository;

    public CommentService(CommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public Comment FindById(Task task, int id)
    {
        List<Comment> comments = task.CommentList;
        foreach (Comment comment in comments)
        {
            if (comment.Id == id)
            {
                return comment;
            }   
        }

        throw new TaskPanelException($"Comment with id: {id} do not exist");
    }

    public Comment DeleteComment(Task task, Comment comment)
    {
        List<Comment> comments = task.CommentList;
        foreach (Comment c in comments)
        {
            if (c.Id == comment.Id)
            {
                comments.Remove(c);
                return c;
            }
            
        }

        throw new TaskPanelException($"Not exist comment with id: {comment.Id}, not deleted");
    }

    public Comment UpdateComment(Task task, Comment comment)
    {
        Comment commentSaved = FindById(task, comment.Id);
        commentSaved.Message = comment.Message ?? commentSaved.Message;
        commentSaved.Status = comment.Status;
        commentSaved.ResolvedAt = comment.ResolvedAt;
        commentSaved.ResolvedBy = comment.ResolvedBy;
        return commentSaved;
    }

    public Comment AddComment(Comment comment)
    {
        VerifyComment(comment);
        _commentRepository.Add(comment);
        return comment;
    }

    public void VerifyComment(Comment comment)
    {
        if (String.IsNullOrEmpty(comment.Message) || comment.Status == null)
        {
            throw new TaskPanelException("Comment not valid");
        }
    }
}