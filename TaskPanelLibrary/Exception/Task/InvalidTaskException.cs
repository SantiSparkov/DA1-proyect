namespace TaskPanelLibrary.Exception.Task;

public class InvalidTaskException : System.Exception
{
    public InvalidTaskException()
        : base($"Task is not valid, please check the task details")
    {
    }
}