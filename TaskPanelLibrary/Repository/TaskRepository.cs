using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Repository.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Repository;

public class TaskRepository : ITaskRepository
{
    private readonly List<Task> _tasks = new List<Task>();

    public Task AddTask(Task task)
    {
        task.Id = _tasks.Count > 0 ? _tasks.Max(t => t.Id) + 1 : 1;
        _tasks.Add(task);
        return task;
    }

    public Task DeleteTask(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id)
                   ?? throw new TaskNotFoundException(id);
        _tasks.Remove(task);
        return task;
    }

    public Task GetTaskById(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id)
                   ?? throw new TaskNotFoundException(id);
        return task;
    }

    public List<Task> GetAllTasks()
    {
        var tasks = _tasks;
        return tasks;
    }

    public Task UpdateTask(Task task)
    {
        var existingTask = _tasks.FirstOrDefault(t => t.Id == task.Id)
                           ?? throw new TaskNotFoundException(task.Id);

        existingTask.Title = task.Title ?? existingTask.Title;
        existingTask.Description = task.Description ?? existingTask.Description;
        existingTask.DueDate = task.DueDate != default(DateTime) ? task.DueDate : existingTask.DueDate;
        existingTask.Priority = task.Priority;

        return existingTask;
    }
}