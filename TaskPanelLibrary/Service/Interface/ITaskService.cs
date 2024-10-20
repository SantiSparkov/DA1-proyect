using Task = TaskPanelLibrary.Entity.Task;
using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Service.Interface;

public interface ITaskService
{
    Task GetTaskById(int id);
    
    List<Task> GetAllTasks(int panelId);
    
    Task CreateTask(Task task);
    
    Task UpdateTask(Task task);

    Task DeleteTask(Task task);
}