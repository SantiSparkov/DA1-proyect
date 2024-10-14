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

    public TaskService(ITaskRepository taskRepository, ICommentService commentService)
    {
        _taskRepository = taskRepository;

        _commentService = commentService;
    }

    public List<Task> GetAllTasks(int panelId)
    {
        return _taskRepository.GetAllTasks();
    }

    public Task AddTask(Task task)
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
        _taskRepository.UpdateTask(existingTask);

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
            throw new CommentNotValidException();
        task.CommentList.Add(comment);
        _taskRepository.UpdateTask(task);
    }

    public void MarkCommentAsDone(int taskId, int commentId)
    {
        var task = _taskRepository.GetTaskById(taskId);
        var existingComment = _commentService.FindById(commentId);

        existingComment.ResolvedAt = DateTime.Now;
        existingComment.Status = EStatusComment.RESOLVED;

        _commentService.UpdateComment(task, existingComment);
        _taskRepository.UpdateTask(task);
    }

    private bool IsValidTask(Task task)
    {
        return task != null && !string.IsNullOrEmpty(task.Title) && !string.IsNullOrEmpty(task.Description);
    }
}