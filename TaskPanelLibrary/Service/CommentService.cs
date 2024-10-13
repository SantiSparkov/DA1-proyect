using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    public CommentService(ICommentRepository commentRepository)
    {
        _commentRepository = commentRepository;
    }

    public Comment CreateComment()
    {
        Comment comment = new Comment()
        {
            Id = _commentRepository.GetAll().Count + 1
        };
        _commentRepository.Add(comment);
        return comment;
    }

    public Comment FindById(int id)
    {
        List<Comment> comments = _commentRepository.GetAll();
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
                _commentRepository.Delete(comment.Id);
                comments.Remove(c);
                return c;
            }
            
        }

        throw new TaskPanelException($"Not exist comment with id: {comment.Id}, not deleted");
    }

    public Comment UpdateComment(Task task, Comment comment)
    {
        Comment commentSaved = FindById(comment.Id);
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