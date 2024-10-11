using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.Comment;
using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

public class TaskService : ITaskService
{
    private readonly TaskRepository taskRepository;
    private PanelService _panelService;
    private CommentService _commentService;
    
    public TaskService(TaskRepository taskRepository, PanelService panelService, CommentService commentService)
    {
        this.taskRepository = taskRepository;
        _panelService = panelService;
        _commentService = commentService;
    }

    public List<Task> GetAllTasks(int panelId)
    {
        var panel = _panelService.FindById(panelId);
        return panel.Tasks;
    }

    public Task AddTask(Task task)
    {
        if (!IsValidTask(task))
            throw new InvalidTaskException();
        
        taskRepository.AddTask(task);

        return task;
    }
    
    public Task GetTaskById(int id)
    {
        var task = taskRepository.GetTaskById(id);
        return task;
    }

    public Task UpdateTask(Task task)
    {
        var existingTask = taskRepository.GetTaskById(task.Id);
        taskRepository.UpdateTask(existingTask);

        return existingTask;
    }

    public Task DeleteTask(Task task)
    {
        var existingTask = taskRepository.GetTaskById(task.Id);
        taskRepository.DeleteTask(existingTask.Id);

        return existingTask;
    }

    public void AddComentToTask(int taskId, Comment comment)
    {
        var task = taskRepository.GetTaskById(taskId);
        if (comment == null)
            throw new CommentNotValidException();
        task.CommentList.Add(comment);
        _commentService.AddComment(comment);
        taskRepository.UpdateTask(task);
    }

    public void MarkCommentAsDone(int taskId, int commentId)
    {
        var task = taskRepository.GetTaskById(taskId);
        var existingComment = _commentService.FindById(task, commentId);
        
        existingComment.ResolvedAt = DateTime.Now;
        existingComment.Status = EStatusComment.RESOLVED;
        
        _commentService.UpdateComment(task, existingComment);
        taskRepository.UpdateTask(task);
    }

    private bool IsValidTask(Task task)
    {
        return task != null && !string.IsNullOrEmpty(task.Title) && !string.IsNullOrEmpty(task.Description);
    }
    
}