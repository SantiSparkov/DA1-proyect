namespace TaskPanelLibrary.Exception.Comment;

public class CommentNotValidException : System.Exception
{
    public CommentNotValidException() 
        : base($"Comment is not valid")
    {
    }
}