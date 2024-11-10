using Microsoft.Data.SqlClient;
using TaskPanelLibrary.Config;

namespace TaskPanelLibrary.Repository;

using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Exception.Comment;
using TaskPanelLibrary.Repository.Interface;

public class CommentSqlRepository : ICommentRepository
{
    private readonly SqlContext _commentDataBase;

    public CommentSqlRepository(SqlContext commentDataBase)
    {
        _commentDataBase = commentDataBase;
    }

    public Comment AddComment(Comment comment)
    {
        _commentDataBase.Comments.Add(comment); 
        _commentDataBase.SaveChanges();
        return comment;
        
    }

    public Comment GetCommentById(int id)
    {
        List<Comment> comments = _commentDataBase.Comments.ToList();
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
        Comment commentSaved = _commentDataBase.Comments.Find(id);
        if (commentSaved != null)
        {
            _commentDataBase.Comments.Remove(commentSaved);
            _commentDataBase.SaveChanges();
            return commentSaved;
        }
        throw new CommentNotValidException($"Comment with id: {id} does not exist");
    }

    public List<Comment> GetAllComments()
    {
        return _commentDataBase.Comments.ToList();
    }

    public Comment UpdateComment(Comment comment)
    {
        _commentDataBase.Update(comment);
        _commentDataBase.SaveChanges();
        return comment;
    }
}