namespace TaskPanelLibrary.Exception.Task;

public class TaskNotFoundException : SystemException
{
    public TaskNotFoundException (int taskId) 
        : base($"Task with id {taskId} not found")
    {
    }
}