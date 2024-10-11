using TaskPanelLibrary.Entity;
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

    private TaskRepository taskRepository;

    private PanelService _panelService;
    
    private CommentService _commentService;
    
    private CommentRepository _commentRepository;

    [TestInitialize]
    public void Initialize()
    {
        _commentService = new CommentService(_commentRepository);
        taskRepository = new TaskRepository();
        _taskService = new TaskService(taskRepository, _panelService, _commentService);
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
    public void AddTask()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Email = "lalo@gmail.com",
            Name = "Lalo",
            LastName = "Landa",
        };

        var newPanel = new Panel
        {
            Name = "Panel 1",
            Description = "Panel 1 description",
        };

        var panel = _panelService.CreatePanel(user);

        var newTask = new Task
        {
            Title = "Task 1",
            Description = "Task 1 description",
            DueDate = DateTime.Now,
        };

        // Act
        var addedTask = _taskService.AddTask(newTask);

        // Assert
        Assert.IsNotNull(addedTask);
        Assert.AreEqual("Task 1", addedTask.Title);
    }

    [TestMethod]
    public void DeleteTask()
    {
        var user = new User
        {
            Id = 1,
            Email = "lalo@gmail.com",
            Name = "Lalo",
            LastName = "Landa",
        };

        var newPanel = new Panel
        {
            Name = "Panel 1",
            Description = "Panel 1 description",
        };

        var panel = _panelService.CreatePanel(user);

        var newTask = new Task
        {
            Title = "Task 1",
            Description = "Task 1 description",
            DueDate = DateTime.Now,
        };

        var task = _taskService.AddTask(newTask);

        // Act
        _taskService.DeleteTask(newTask);
        
        // Assert
        var deletedTask = _taskService.GetTaskById(task.Id);
        Assert.IsNull(deletedTask);

    }
}