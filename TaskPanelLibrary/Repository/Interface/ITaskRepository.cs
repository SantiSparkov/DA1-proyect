using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Repository.Interface;

public interface ITaskRepository
{
    Task GetTaskById(int id);
    
    List<Task> GetAllTasks();
    
    Task AddTask(Task task);
    
    Task UpdateTask(Task task);
    
    void DeleteTask(int id);

}