namespace TaskPanelLibrary.Exception.Task;

public class TaskNotValidException : System.Exception
{
    public TaskNotValidException(int id) 
        : base($"Task with id {id} not found")
    {
    }
    
    public TaskNotValidException(string message) 
        : base(message)
    {
    }
    
}