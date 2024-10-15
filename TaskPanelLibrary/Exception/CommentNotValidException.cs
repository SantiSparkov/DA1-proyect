namespace TaskPanelLibrary.Exception.Comment;

public class CommentNotValidException : System.Exception
{
    public CommentNotValidException(string message)
        : base(message)
    {
    }
}