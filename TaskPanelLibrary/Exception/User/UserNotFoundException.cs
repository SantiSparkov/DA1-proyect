namespace TaskPanelLibrary.Exception.User;

public class UserNotFoundException : System.Exception
{
    public UserNotFoundException(string email)
        : base($"User with email {email} was not found.")
    {
    }
}
