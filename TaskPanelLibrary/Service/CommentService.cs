using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Exception.Comment;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _commentRepository;

    private IUserService _userService;

    public CommentService(ICommentRepository commentRepository, IUserService userService)
    {
        _commentRepository = commentRepository;
        _userService = userService;
    }

    public Comment CreateComment(Comment comment)
    {
        if (!IsValidComment(comment))
            throw new CommentNotValidException("Comment not valid");
        
        _commentRepository.AddComment(comment);
        return comment;
    }

    public Comment GetCommentById(int id)
    {
        List<Comment> comments = _commentRepository.GetAllComments();
        foreach (Comment comment in comments)
        {
            if (comment.Id == id)
            {
                return comment;
            }   
        }

        throw new CommentNotValidException($"Comment with id: {id} do not exist");
    }

    public Comment DeleteComment(Task task, Comment comment)
    {
        List<Comment> comments = task.CommentList;
        foreach (Comment c in comments)
        {
            if (c.Id == comment.Id)
            {
                _commentRepository.DeleteComment(comment.Id);
                comments.Remove(c);
                return c;
            }
            
        }

        throw new CommentNotValidException($"Not exist comment with id: {comment.Id}, not deleted");
    }

    public Comment UpdateComment(Comment comment)
    {
        Comment commentSaved = GetCommentById(comment.Id);
        commentSaved.Message = comment.Message ?? commentSaved.Message;
        commentSaved.Status = comment.Status;
        commentSaved.ResolvedAt = comment.ResolvedAt;
        commentSaved.ResolvedBy = comment.ResolvedBy;
        return commentSaved;
    }
    
    public List<Comment> GetAllComments()
    {
        return _commentRepository.GetAllComments();
    }

    public List<Comment> GetCommentForTask(int taskId)
    {
        return _commentRepository.GetAllComments().Where(i => i.TaskId == taskId).ToList();
    }

    private bool IsValidComment(Comment comment)
    {
        if (comment == null)
            throw new CommentNotValidException("Comment is null");
        
        if (string.IsNullOrEmpty(comment.Message))
            throw new CommentNotValidException("Comment message is null or empty");
        
        if (comment.Status == EStatusComment.RESOLVED && comment.ResolvedBy == null)
            throw new CommentNotValidException("Comment resolved by is null");
        
        if (comment.Status == null)
            throw new CommentNotValidException("Comment status is null");
        
        return true;
    }
}