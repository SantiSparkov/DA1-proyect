namespace TaskPanelLibrary.Exception.User;

public class UserNotValidException : SystemException
{
    public UserNotValidException(string message)
        : base(message)
    {
    }
    
    public UserNotValidException(int id)
        : base($"User with id {id} not found")
    {
    }
    
}