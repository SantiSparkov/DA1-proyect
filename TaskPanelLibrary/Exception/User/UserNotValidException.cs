namespace TaskPanelLibrary.Exception.User;

public class UserNotValidException : SystemException
{
    public UserNotValidException(string message)
        : base(message)
    {
    }
}