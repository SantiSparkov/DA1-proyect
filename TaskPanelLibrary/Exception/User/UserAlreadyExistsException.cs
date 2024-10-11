namespace TaskPanelLibrary.Exception.User;

public class UserAlreadyExistsException : System.Exception
{
    public UserAlreadyExistsException(string email)
        : base($"User with email {email} already exists.")
    {
    }
    
    public UserAlreadyExistsException(int id)
        : base($"User with id {id} already exists.")
    {
    }
}
