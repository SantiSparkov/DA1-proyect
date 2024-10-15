using TaskPanelLibrary.Entity;
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
        comment.Id = _commentRepository.GetAll().Count + 1;
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

        throw new CommentNotValidException($"Comment with id: {id} do not exist");
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

        throw new CommentNotValidException($"Not exist comment with id: {comment.Id}, not deleted");
    }

    public Comment UpdateComment(Comment comment)
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
    
    public List<Comment> GetAllComments()
    {
        return _commentRepository.GetAll();
    }

    public List<Comment> GetCommentForTask(int taskId)
    {
        return _commentRepository.GetAll().Where(i => i.TaskId == taskId).ToList();
    }

    public void VerifyComment(Comment comment)
    {
        if (String.IsNullOrEmpty(comment.Message) || comment.Status == null)
        {
            throw new CommentNotValidException("Comment not valid");
        }
    }
}