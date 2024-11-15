namespace TaskPanelLibrary.Exception.Comment;

public class NotificationNotValidException : System.Exception
{
    public NotificationNotValidException(string message)
        : base(message)
    {
    }
}