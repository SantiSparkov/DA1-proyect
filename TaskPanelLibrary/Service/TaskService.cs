using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.Comment;
using TaskPanelLibrary.Exception.Panel;
using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

public class TaskService : ITaskService
{
    private readonly IPanelRepository _panelRepository = new PanelRepository();
    private readonly ITaskRepository _taskRepository = new TaskRepository();

    public Task GetTaskById(int id)
    {
        var task = BringExistingTask(id);
        return task;
    }

    public List<Task> GetAllTasks(int panelId)
    {
        var panel = BringExistingPanel(panelId);
        return panel.Tasks;
    }

    public Task AddTask(Task task, int panelId)
    {
        var panel = BringExistingPanel(panelId);
        if (!IsValidTask(task))
            throw new InvalidTaskException();
        
        task.PanelId = panelId;
        panel.Tasks.Add(task);

        _taskRepository.AddTask(task);
        _panelRepository.Update(panel);

        return task;
    }

    public Task UpdateTask(Task task)
    {
        var existingTask = BringExistingTask(task.Id);
        _taskRepository.UpdateTask(existingTask);

        return existingTask;
    }

    public Task DeleteTask(int id, int panelId)
    {
        var panel = BringExistingPanel(panelId);
        var existingTask = BringExistingTask(id);

        panel.Tasks.Remove(existingTask);
        _taskRepository.DeleteTask(existingTask.Id);
        _panelRepository.Update(panel);

        return existingTask;
    }

    public void AddComentToTask(int taskId, Comment comment)
    {
        var task = BringExistingTask(taskId);
        if (comment == null)
            throw new CommentNotValidException();
        task.CommentList.Add(comment);
        _taskRepository.UpdateTask(task);
    }

    private bool IsValidTask(Task task)
    {
        return task != null && !string.IsNullOrEmpty(task.Title) && !string.IsNullOrEmpty(task.Description);
    }
    
    private Panel BringExistingPanel(int panelId)
    {
        var panel = _panelRepository.FindById(panelId);
        if (panel == null)
            throw new PanelNotFoundException(panelId);
        
        return panel;
    }
    
    private Task BringExistingTask(int taskId)
    {
        var task = _taskRepository.GetTaskById(taskId);
        if (task == null)
            throw new TaskNotFoundException(task.Id);
        
        return task;
    }
}