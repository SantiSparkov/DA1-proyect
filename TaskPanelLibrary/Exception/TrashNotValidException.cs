namespace TaskPanelLibrary.Exception;

public class TrashNotValidException : System.Exception
{
    public TrashNotValidException(string message)
        : base(message)
    {
    }
    
    public TrashNotValidException(int id)
        : base($"Trash with id {id} not found")
    {
    }
}