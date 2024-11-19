using Moq;
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

    private Mock<ITaskRepository> _taskRepository;
    
    private Mock<IUserService> _userService;
    
    private Mock<ITrashService> _trashService;
    
    private Task _task;

    [TestInitialize]
    public void Initialize()
    {
        _taskRepository = new Mock<ITaskRepository>();
        _trashService = new Mock<ITrashService>();
        _userService = new Mock<IUserService>();
        _taskService = new TaskService(_taskRepository.Object, _userService.Object, _trashService.Object);
        
        _task = new Task()
        {
            Id = 1,
            PanelId = 1,
            Title = "Title test",
            Description = "Description test",
            Priority = EPriority.LOW,
            DueDate = new DateTime(DateTime.Now.Year+1, DateTime.Now.Month, DateTime.Now.Day)
        };
    }

    [TestCleanup]
    public void Cleanup()
    {
        
    }

    [TestMethod]
    public void CreateTask()
    {
        // Arrange

        // Act
        _taskRepository.Setup(service => service.AddTask(It.IsAny<Task>()));
        Task createdTask = _taskService.CreateTask(_task);
        
        // Assert
        Assert.IsNotNull(createdTask);
        Assert.AreEqual("Description test", createdTask.Description);
        Assert.AreEqual(EPriority.LOW, createdTask.Priority);
        Assert.AreEqual("Title test", createdTask.Title);
    }
    
    [TestMethod]
    public void CreateTaskNull()
    {
        // Arrange

        // Act
        var exception = Assert.ThrowsException<TaskNotValidException>(() => _taskService.CreateTask(null));
        
        // Assert
        Assert.AreEqual("Task is null", exception.Message);
    }
    
    [TestMethod]
    public void CreateTaskNullTitle()
    {
        // Arrange
        Task task = new Task()
        {
            Title = "",
            Description = "Desc test"
        };
        // Act
        var exception = Assert.ThrowsException<TaskNotValidException>(() => _taskService.CreateTask(task));
        
        // Assert
        Assert.AreEqual("Title is null or empty", exception.Message);
    }
    
    [TestMethod]
    public void CreateTaskDueDate()
    {
        // Arrange
        Task task = new Task()
        {
            Title = "Test",
            Description = "Desc test",
            DueDate = new DateTime(2024, 10, 12)
        };
        // Act
        var exception = Assert.ThrowsException<TaskNotValidException>(() => _taskService.CreateTask(task));
        
        // Assert
        Assert.AreEqual("Due date is before today", exception.Message);
    }
    
    [TestMethod]
    public void CreateTaskDescriptionNull()
    {
        // Arrange
        Task task = new Task()
        {
            Title = "Test",
            DueDate = new DateTime(2025, 12, 12)
        };
        // Act
        var exception = Assert.ThrowsException<TaskNotValidException>(() => _taskService.CreateTask(task));
        
        // Assert
        Assert.AreEqual("Description is null or empty", exception.Message);
    }

    [TestMethod]
    public void DeleteTask()
    {
        // Arrange
        _taskRepository.Setup(service => service.AddTask(It.IsAny<Task>()));
        Task createdTask = _taskService.CreateTask(_task);
        User user = new User()
        {
            Id = 1,
            Email = "test@mail.com",
            Name = "test"
        };
        // Act
        _task.IsDeleted = false;
        _taskRepository.Setup(service => service.GetTaskById(It.IsAny<int>()))
            .Returns(_task);
        _trashService.Setup(service => service.RemoveTaskFromTrash(It.IsAny<int>(),It.IsAny<int>()));
        _taskRepository.Setup(service => service.DeleteTask(It.IsAny<int>()));
    
        Task taskDeleted = _taskService.DeleteTask(createdTask, user);
        
        // Assert
        Assert.AreEqual(createdTask.Id, taskDeleted.Id);
    }
    
    [TestMethod]
    public void DeleteTaskIsDeleted()
    {
        // Arrange
        _taskRepository.Setup(service => service.AddTask(It.IsAny<Task>()));
        Task createdTask = _taskService.CreateTask(_task);
        User user = new User()
        {
            Id = 1,
            Email = "test@mail.com",
            Name = "test"
        };
        // Act
        _task.IsDeleted = true;
        _taskRepository.Setup(service => service.GetTaskById(It.IsAny<int>()))
            .Returns(_task);
        _trashService.Setup(service => service.RemoveTaskFromTrash(It.IsAny<int>(),It.IsAny<int>()));
        _taskRepository.Setup(service => service.DeleteTask(It.IsAny<int>()));
    
        Task taskDeleted = _taskService.DeleteTask(createdTask, user);
        
        // Assert
        Assert.AreEqual(createdTask.Id, taskDeleted.Id);
    }

    [TestMethod]
    public void UpdateTask()
    {
        // Arrange
        Task createdTask = _taskService.CreateTask(_task);
        
        // Act
        createdTask.Description = "Description test updated";
        createdTask.Priority = EPriority.HIGH;
        createdTask.Title = "Title test updated";
        _taskRepository.Setup(service => service.GetTaskById(It.IsAny<int>()))
            .Returns(_task);
        _taskRepository.Setup(service => service.UpdateTask(It.IsAny<Task>()))
            .Returns(_task);
        Task updatedTask = _taskService.UpdateTask(createdTask);
        
        // Assert
        Assert.IsNotNull(updatedTask);
        Assert.AreEqual("Description test updated", updatedTask.Description);
        Assert.AreEqual(EPriority.HIGH, updatedTask.Priority);
        Assert.AreEqual("Title test updated", updatedTask.Title);
    }
    
    [TestMethod]
    public void GetTaskById()
    {
        // Arrange
        _taskRepository.Setup(service => service.AddTask(It.IsAny<Task>()));
        Task createdTask = _taskService.CreateTask(_task);
        
        // Act
        _taskRepository.Setup(service => service.GetTaskById(It.IsAny<int>()))
            .Returns(createdTask);
        Task task = _taskService.GetTaskById(createdTask.Id);
        
        // Assert
        Assert.IsNotNull(task);
        Assert.AreEqual(createdTask.Id, task.Id);
    }
    
    [TestMethod]
    public void GetTasksForPanel()
    {
        // Arrange
        _taskRepository.Setup(service => service.AddTask(It.IsAny<Task>()));
        Task createdTask = _taskService.CreateTask(_task);
        List<Task> tasks = new List<Task>();
        tasks.Add(createdTask);

        // Act
        _taskRepository.Setup(service => service.GetAllTasks())
            .Returns(tasks);
        List<Task> tasksForPanel = _taskService.GetTasksFromPanel(createdTask.PanelId);
        
        // Assert
        Assert.IsNotNull(tasksForPanel);
        Assert.AreEqual(1, tasksForPanel.Count);
    }
    
    [TestMethod]
    public void GetTasksForPanelNull()
    {
        // Arrange
        List<Task> list = null;
        
        // Act
        _taskRepository.Setup(service => service.GetAllTasks())
            .Returns(list);
        
        List<Task> tasksForPanel = _taskService.GetTasksFromPanel(1);
        
        // Assert
        Assert.IsNotNull(tasksForPanel);
        Assert.AreEqual(0, tasksForPanel.Count);
    }
    
    [TestMethod]
    public void GetTasksForEpic()
    {
        // Arrange
        _taskRepository.Setup(service => service.AddTask(It.IsAny<Task>()));
        _task.EpicId = 1;
        Task createdTask = _taskService.CreateTask(_task);
        List<Task> tasks = new List<Task>();
        tasks.Add(createdTask);

        // Act
        _taskRepository.Setup(service => service.GetAllTasks())
            .Returns(tasks);
        List<Task> tasksFromEpic = _taskService.GetTasksFromEpic(createdTask.PanelId);
        
        // Assert
        Assert.IsNotNull(tasksFromEpic);
        Assert.AreEqual(1, tasksFromEpic.Count);
    }
    
    [TestMethod]
    public void GetTasksForEpicEmpty()
    {
        // Arrange
        List<Task> list = null;
        
        // Act
        _taskRepository.Setup(service => service.GetAllTasks())
            .Returns(list);
        List<Task> tasksFromEpic = _taskService.GetTasksFromEpic(1);
        
        // Assert
        Assert.IsNotNull(tasksFromEpic);
        Assert.AreEqual(0, tasksFromEpic.Count);
    }
    
    [TestMethod]
    public void RecoverTask()
    {
        // Arrange
        _taskRepository.Setup(service => service.AddTask(It.IsAny<Task>()));
        Task createdTask = _taskService.CreateTask(_task);
        _taskRepository.Setup(service => service.GetTaskById(It.IsAny<int>()))
            .Returns(_task);
        List<Task> tasks = new List<Task>();
        tasks.Add(createdTask);
        Trash trash = new Trash()
        {
            Id = 1,
            UserId = 1,
            TaskList = tasks
        };
        User user = new User()
        {
            Id = 1,
            Name = "Name",
            LastName = "Test",
            TrashId = 1
        };
        _trashService.Setup(service => service.GetTrashById(It.IsAny<int>()))
            .Returns(trash);

        // Act
        _trashService.Setup(service => service.RecoverTaskFromTrash(It.IsAny<int>(), It.IsAny<int>()));
        createdTask.IsDeleted = false;
        _taskRepository.Setup(service => service.UpdateTask(It.IsAny<Task>()))
            .Returns(createdTask);
        _taskService.RecoverTask(createdTask, user);
        
        // Assert
        Assert.IsNotNull(createdTask);
        Assert.IsFalse(createdTask.IsDeleted);
    }
    
    [TestMethod]
    public void GetAllTasks()
    {
        // Arrange
        _taskRepository.Setup(service => service.AddTask(It.IsAny<Task>()));
        Task createdTask = _taskService.CreateTask(_task);
        _taskRepository.Setup(service => service.GetTaskById(It.IsAny<int>()))
            .Returns(_task);
        List<Task> tasks = new List<Task>();
        tasks.Add(createdTask);

        // Act
        _taskRepository.Setup(service => service.GetAllTasks()).Returns(tasks);
        List<Task> allTasks = _taskService.GetAllTasks();
        
        // Assert
        Assert.IsNotNull(allTasks);
        Assert.AreEqual(1, allTasks.Count);
    }
    
}