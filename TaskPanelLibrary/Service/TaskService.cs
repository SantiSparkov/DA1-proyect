using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Exception.Comment;
using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelLibrary.Service;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _taskRepository;

    private readonly ICommentService _commentService;

    private readonly IPanelService _panelService;

    public TaskService(ITaskRepository taskRepository, ICommentService commentService, IPanelService panelService)
    {
        _taskRepository = taskRepository;

        _commentService = commentService;
        _panelService = panelService;
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

    public Task CreateTask(Task task)
    {
        if (!IsValidTask(task))
            throw new TaskNotValidException(task.Id);

        _taskRepository.AddTask(task);
        return task;
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

    public Task DeleteTask(Task task)
    {
        var existingTask = _taskRepository.GetTaskById(task.Id);
        _taskRepository.DeleteTask(existingTask.Id);

        return existingTask;
    }

    public void AddComentToTask(int taskId, Comment comment)
    {
        var task = _taskRepository.GetTaskById(taskId);
        if (comment == null)
            throw new CommentNotValidException("Comment is null");
        task.CommentList.Add(comment);
        _taskRepository.UpdateTask(task);
    }

    public void MarkCommentAsDone(int taskId, int commentId)
    {
        var task = _taskRepository.GetTaskById(taskId);
        var existingComment = _commentService.FindById(commentId);

        existingComment.ResolvedAt = DateTime.Now;
        existingComment.Status = EStatusComment.RESOLVED;

        _commentService.UpdateComment(existingComment);
        _taskRepository.UpdateTask(task);
    }

    private bool IsValidTask(Task? task)
    {
        if(task == null)
            throw new TaskNotValidException("Task is null");
        if (string.IsNullOrEmpty(task.Title))
            throw new TaskNotValidException("Title is null or empty");
        if (string.IsNullOrEmpty(task.Description))
            throw new TaskNotValidException("Description is null or empty");
        if (task.DueDate < DateTime.Now)
            throw new TaskNotValidException("DueDate is less than current date");
        
        return true;
    }
}