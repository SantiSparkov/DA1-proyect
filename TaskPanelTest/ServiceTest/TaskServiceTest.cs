using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.ServiceTest;

[TestClass]
public class TaskServiceTest
{
    private ITaskService _taskService;

    private ITaskRepository taskRepository;

    private IPanelService _panelService;
    
    private IPanelRepository _panelRepository;
    
    private IUserRepository _userRepository;
    
    private IUserService _userService;
    
    private ICommentService _commentService;
    
    private ICommentRepository _commentRepository;
    
    private PasswordGeneratorService _passwordGeneratorService;
    
    private Task _task;

    [TestInitialize]
    public void Initialize()
    {
        _commentRepository = new CommentRepository();
        _userRepository = new UserRepository();
        _panelRepository = new PanelRepository();
        taskRepository = new TaskRepository();
        _passwordGeneratorService = new PasswordGeneratorService();
        
        _userService = new UserService(_userRepository, _passwordGeneratorService);
        _commentService = new CommentService(_commentRepository, _userService);
        _panelService = new PanelService(_panelRepository, _userService);
        _taskService = new TaskService(taskRepository, _commentService, _panelService);
        
        _task = new Task()
        {
            Id = 1,
            PanelId = 1,
            Title = "Title test",
            Description = "Description test",
            Priority = ETaskPriority.LOW
        };
    }

    [TestCleanup]
    public void Cleanup()
    {
        var panels = _panelRepository.GetAll().ToList();
        var comments = _commentRepository.GetAll().ToList();
        var tasks = taskRepository.GetAllTasks().ToList();
        var users = _userRepository.GetAllUsers().ToList();
        _passwordGeneratorService = null;

        foreach (var panel in panels)
        {
            _panelRepository.Delete(panel.Id);
        }
        foreach (var comment in comments)
        {
            _commentRepository.Delete(comment.Id);
        }
        foreach (var task in tasks)
        {
            taskRepository.DeleteTask(task.Id);
        }
        foreach (var user in users)
        {
            _userRepository.DeleteUser(user.Id);
        }
    }

    [TestMethod]
    public void CreateTask()
    {
        // Arrange
        var createdTask = _taskService.CreateTask(_task);
        
        // Act
        createdTask.Description = "Description test";
        createdTask.Priority = ETaskPriority.LOW;
        createdTask.Title = "Title test";
        
        // Assert
        Assert.IsNotNull(createdTask, "The task is not created");
        Assert.AreEqual("Description test", createdTask.Description, "The task description is not stored correctly");
        Assert.AreEqual(ETaskPriority.LOW, createdTask.Priority, "The task priority is not stored correctly");
        Assert.AreEqual("Title test", createdTask.Title, "The task title is not stored correctly");
    }

    [TestMethod]
    public void DeleteTask()
    {
        // Arrange
        var createdTask = _taskService.CreateTask(_task);
        
        // Act
        _taskService.DeleteTask(createdTask);
        
        // Assert
        var exception = Assert.ThrowsException<TaskNotValidException>(() => _taskService.GetTaskById(createdTask.Id));
        Assert.AreEqual(exception.Message, $"Task with id {createdTask.Id} not found");
    }

    [TestMethod]
    public void UpdateTask()
    {
        // Arrange
        var createdTask = _taskService.CreateTask(_task);
        
        // Act
        createdTask.Description = "Description test updated";
        createdTask.Priority = ETaskPriority.HIGH;
        createdTask.Title = "Title test updated";
        var updatedTask = _taskService.UpdateTask(createdTask);
        
        // Assert
        Assert.IsNotNull(updatedTask, "The task is not updated");
        Assert.AreEqual("Description test updated", updatedTask.Description, "The task description is not updated correctly");
        Assert.AreEqual(ETaskPriority.HIGH, updatedTask.Priority, "The task priority is not updated correctly");
        Assert.AreEqual("Title test updated", updatedTask.Title, "The task title is not updated correctly");
    }
    
    [TestMethod]
    public void AddCommentToTask()
    {
        // Arrange
        var createdTask = _taskService.CreateTask(_task);
        var comment = new Comment()
        {
            Id = 1,
            TaskId = createdTask.Id,
            Message = "Comment test"
        };
        var createdComment = _commentService.CreateComment(comment);
        
        // Act
        _taskService.AddComentToTask(createdTask.Id, createdComment);
        
        // Assert
        var task = _taskService.GetTaskById(createdTask.Id);
        Assert.AreEqual(1, task.CommentList.Count);
        Assert.AreEqual("Comment test", task.CommentList.First().Message);
    }
    
    [TestMethod]
    public void MarkCommentAsDone()
    {
        // Arrange
        var createdTask = _taskService.CreateTask(_task);
        var comment = new Comment()
        {
            Id = 1,
            TaskId = createdTask.Id,
            Message = "Comment test"
        };
        var createdComment = _commentService.CreateComment(comment);
        _taskService.AddComentToTask(createdTask.Id, createdComment);
        
        // Act
        _taskService.MarkCommentAsDone(createdTask.Id, createdComment.Id);
        
        // Assert
        var task = _taskService.GetTaskById(createdTask.Id);
        Assert.AreEqual(1, task.CommentList.Count);
        Assert.AreEqual(EStatusComment.RESOLVED, task.CommentList.First().Status);
    }
    
    [TestMethod]
    public void GetTaskById()
    {
        // Arrange
        var createdTask = _taskService.CreateTask(_task);
        
        // Act
        var task = _taskService.GetTaskById(createdTask.Id);
        
        // Assert
        Assert.IsNotNull(task, "The task is not found");
        Assert.AreEqual(createdTask.Id, task.Id, "The task id is not found correctly");
    }
}