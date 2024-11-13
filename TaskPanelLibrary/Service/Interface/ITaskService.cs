using Task = TaskPanelLibrary.Entity.Task;
using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Service.Interface;

public interface ITaskService
{
    public Task GetTaskById(int id);
    
    public List<Task> GetAllTasks(int panelId);
    
    public Task CreateTask(Task task);
    
    public Task UpdateTask(Task task);

    public Task DeleteTask(Task task, User user);
    
    public Task RecoverTask(Task task, User user);
}