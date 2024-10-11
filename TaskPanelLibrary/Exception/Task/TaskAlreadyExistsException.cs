namespace TaskPanelLibrary.Exception.Task;

public class TaskAlreadyExistsException : System.Exception
{
    public TaskAlreadyExistsException(int id) 
        : base($"Task with id {id} already exists")
    {
    }
}