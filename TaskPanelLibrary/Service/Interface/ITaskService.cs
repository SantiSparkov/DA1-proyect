using Task = TaskPanelLibrary.Entity.Task;
using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Service.Interface;

public interface ITaskService
{
    public Task GetTaskById(int id);
    
    public List<Task> GetTasksFromPanel(int panelId);
    
    public Task CreateTask(Task task);
    
    public Task UpdateTask(Task task);

    public Task DeleteTask(Task task, User user);
    
    public Task RecoverTask(Task task, User user);
    
    public List<Task> GetAllTasks();
    
    public List<Task> GetTasksFromEpic(int epicId);
}