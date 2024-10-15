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
        var panels = _panelService.GetAllPanels();
        var panelsToDelete = panels.ToList();
        foreach (var panel in panelsToDelete)
        {
            foreach (var task in panel.Tasks)
            {
                _taskService.DeleteTask(task);
            }
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
}