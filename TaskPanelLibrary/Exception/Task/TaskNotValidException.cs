namespace TaskPanelLibrary.Exception.Task;

public class TaskNotValidException : System.Exception
{
    public TaskNotValidException(int id) 
        : base($"La tarea con id {id} no es válida")
    {
    }
    
    public TaskNotValidException(string message) 
        : base(message)
    {
    }
    
}