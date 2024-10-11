using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.ServiceTest;

[TestClass]
public class TaskServiceTest
{
    private ITaskService _taskService;
    private IPanelService _panelService;

    [TestInitialize]
    public void Initialize()
    {
        _taskService = new TaskService();
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
                _taskService.DeleteTask(task.Id, panel.Id);
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
        var addedTask = _taskService.AddTask(newTask, panel.Id);

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

        var task = _taskService.AddTask(newTask, panel.Id);

        // Act
        _taskService.DeleteTask(task.Id, panel.Id);
        
        // Assert
        var deletedTask = _taskService.GetTaskById(task.Id);
        Assert.IsNull(deletedTask);

    }
}