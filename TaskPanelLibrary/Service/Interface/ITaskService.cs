using Microsoft.Testing.Platform.Extensions.Messages;
using Task = TaskPanelLibrary.Entity.Task;
using TaskPanelLibrary.Entity;

namespace TaskPanelLibrary.Service.Interface;

public interface ITaskService
{
    Task GetTaskById(int id);
    
    List<Task> GetAllTasks(int panelId);
    
    Task AddTask(Task task, int panelId);
    
    Task UpdateTask(Task task);
    
    Task DeleteTask(int id, int panelId);
    
    void AddComentToTask(int taskId, Comment comment);
}