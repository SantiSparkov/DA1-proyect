using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Repository.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Repository;

public class TaskRepository : ITaskRepository
{
    private List<Task> _tasks;

    public TaskRepository()
    {
        _tasks = new List<Task>();
    }

    public Task GetTaskById(int id)
    {
        var task = _tasks.FirstOrDefault(t => t.Id == id);
        if (task.Id == id)
        {
            return task;
        }

        throw new TaskNotFoundException(id);
    }

    public List<Task> GetAllTasks()
    {
        return _tasks;
    }

    public Task AddTask(Task task)
    {
        _tasks.Add(task);
        return task;
    }

    public Task UpdateTask(Task task)
    {
        var exsistingTask = _tasks.FirstOrDefault(t => t.Id == task.Id);
        if (exsistingTask.Id == task.Id)
        {
            exsistingTask.Title = task.Title ?? exsistingTask.Title;
            exsistingTask.Description = task.Description ?? exsistingTask.Description;
            exsistingTask.DueDate = task.DueDate != default(DateTime) ? task.DueDate : exsistingTask.DueDate;
            exsistingTask.Priority = task.Priority;
            return exsistingTask;
        }

        throw new TaskNotFoundException(task.Id);
    }

    public void DeleteTask(int id)
    {
        var task = GetTaskById(id);
        if (task.Id == id)
        {
            _tasks.Remove(task);
        }
        else
        {
            throw new TaskNotFoundException(id);
        }
    }
}