using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service;

public class CommentService : ICommentService
{
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
}