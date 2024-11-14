using Microsoft.Identity.Client;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Exception.Comment;
using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ITrashService _trashService;
    private readonly IUserService _userService;

    public TaskService(ITaskRepository taskSqlRepository, IUserService userService, ITrashService trashService)
    {
        _taskRepository = taskSqlRepository;
        _userService = userService;
        _trashService = trashService;
    }

    public Task CreateTask(Task task)
    {
        if (!IsValidTask(task))
            throw new TaskNotValidException(task.Id);

        _taskRepository.AddTask(task);
        return task;
    }

    public List<Task> GetAllTasks(int panelId)
    {
        try
        {
            return _taskRepository.GetAllTasks().Where(i => i.PanelId == panelId).ToList();
        }
        catch (ArgumentException e)
        {
            return new List<Task>();
        }
    }

    public Task GetTaskById(int id)
    {
        var task = _taskRepository.GetTaskById(id);
        return task;
    }

    public Task UpdateTask(Task task)
    {
        var existingTask = _taskRepository.GetTaskById(task.Id);
        _taskRepository.UpdateTask(task);
        return existingTask;
    }

    public Task DeleteTask(Task task, User user)
    {
        var existingTask = _taskRepository.GetTaskById(task.Id);

        if (existingTask.IsDeleted)
        {
            _trashService.RemoveTaskFromTrash(existingTask.Id, user.TrashId);
            _taskRepository.DeleteTask(existingTask.Id);
        }
        else
        {
            existingTask.IsDeleted = true;
            _trashService.AddTaskToTrash(existingTask, user.TrashId);
            _taskRepository.UpdateTask(existingTask);
        }
        

        return existingTask;
    }
    
    public Task RecoverTask(Task task, User user)
    {
        var existingTask = _taskRepository.GetTaskById(task.Id);

        if (_trashService.GetTrashById(user.TrashId).TaskList.Contains(existingTask))
        {
            _trashService.RecoverTaskFromTrash(existingTask.Id, user.TrashId);
            existingTask.IsDeleted = false;
            _taskRepository.UpdateTask(existingTask);
        }
        
        return existingTask;
    }

    private bool IsValidTask(Task? task)
    {
        if (task == null)
            throw new TaskNotValidException("Task is null");
        if (string.IsNullOrEmpty(task.Title))
            throw new TaskNotValidException("Title is null or empty");
        if (string.IsNullOrEmpty(task.Description))
            throw new TaskNotValidException("Description is null or empty");
        if (task.DueDate < DateTime.Now)
            throw new TaskNotValidException("Due date is before today");

        return true;
    }
}