using TaskPanelLibrary.Config;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelTest.ConfigTest;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.RepositoryTest.SqlRepositories;

[TestClass]
public class TaskSqlRepositoryTest
{
    private SqlContext _sqlContext;

    private ITaskRepository _taskRepository;
    
    [TestInitialize]
    public void Initialize()
    {
        _sqlContext = new SqlContexTest().CreateMemoryContext();
        _taskRepository = new TaskSqlRepository(_sqlContext);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext?.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void AddTask()
    {
        //Arrange
        Task task = new Task()
        {
            Id = 1,
            Title = "Task title",
            CommentList = new List<Comment>(),
            Description = "Desc task",
            DueDate = new DateTime(2024, 11, 20),
            EpicId = 1,
            EstimatioHour = 10,
            InvertedEstimateHour = 2,
            Priority = EPriority.MEDIUM, IsDeleted = false,
            PanelId = 1
        };
        
        //Act
        _taskRepository.AddTask(task);

        //Assert
        Assert.AreEqual(1, _taskRepository.GetAllTasks().Count);
    }
    
    [TestMethod]
    public void GetTaskById()
    {
        //Arrange
        Task task = new Task()
        {
            Id = 1,
            Title = "Task title",
            CommentList = new List<Comment>(),
            Description = "Desc task",
            DueDate = new DateTime(2024, 11, 20),
            EpicId = 1,
            EstimatioHour = 10,
            InvertedEstimateHour = 2,
            Priority = EPriority.MEDIUM, IsDeleted = false,
            PanelId = 1
        };
        
        //Act
        _taskRepository.AddTask(task);

        Task taskSaved = _taskRepository.GetTaskById(task.Id);

        //Assert
        Assert.AreEqual(task.Id, taskSaved.Id);
        Assert.AreEqual(task.Title, taskSaved.Title);
        Assert.AreEqual(task.Description, taskSaved.Description);
        Assert.AreEqual(task.PanelId, taskSaved.PanelId);
        Assert.AreEqual(task.EstimatioHour, taskSaved.EstimatioHour);
        Assert.AreEqual(task.InvertedEstimateHour, taskSaved.InvertedEstimateHour);
        Assert.AreEqual(task.EpicId, taskSaved.EpicId);
        Assert.AreEqual(task.DueDate, taskSaved.DueDate);
        Assert.AreEqual(task.Priority, taskSaved.Priority);
        Assert.IsNotNull(taskSaved.CommentList);
    }
    
    [TestMethod]
    public void GetTeamByIdNotExist()
    {
        //Arrange

        //Act 
        var exception = Assert.ThrowsException<System.Exception>(() => _taskRepository.GetTaskById(1));

        // Assert
        Assert.AreEqual($"Task with id: {1} does not exist", exception.Message);
    }
    
    [TestMethod]
    public void UpdateTask()
    {
        //Arrange
        Task task = new Task()
        {
            Id = 1,
            Title = "Task title",
            CommentList = new List<Comment>(),
            Description = "Desc task",
            DueDate = new DateTime(2024, 11, 20),
            EpicId = 1,
            EstimatioHour = 10,
            InvertedEstimateHour = 2,
            Priority = EPriority.MEDIUM, IsDeleted = false,
            PanelId = 1
        };
        
        //Act
        _taskRepository.AddTask(task);
        task.Description = "Updated description";
        task.Title = "Title updated";
        task.InvertedEstimateHour = 5;
        
        Task taskSaved = _taskRepository.UpdateTask(task);

        //Assert
        Assert.AreEqual("Title updated", taskSaved.Title);
        Assert.AreEqual("Updated description", taskSaved.Description);
        Assert.AreEqual(5, taskSaved.InvertedEstimateHour);
    }
    
    [TestMethod]
    public void DeleteTask()
    {
        //Arrange
        Task task = new Task()
        {
            Id = 1,
            Title = "Task title",
            CommentList = new List<Comment>(),
            Description = "Desc task",
            DueDate = new DateTime(2024, 11, 20),
            EpicId = 1,
            EstimatioHour = 10,
            InvertedEstimateHour = 2,
            Priority = EPriority.MEDIUM, IsDeleted = false,
            PanelId = 1
        };
        
        //Act
        _taskRepository.AddTask(task);
        
        _taskRepository.DeleteTask(task.Id);
        
        var exception = Assert.ThrowsException<System.Exception>(() => _taskRepository.GetTaskById(task.Id));

        // Assert
        Assert.AreEqual($"Task with id: {1} does not exist", exception.Message);
    }
    
    [TestMethod]
    public void DeleteTaskNotExist()
    {
        //Arrange

        //Act
        var exception = Assert.ThrowsException<System.Exception>(() => _taskRepository.DeleteTask(1));

        // Assert
        Assert.AreEqual($"Task with id: {1} does not exist", exception.Message);
    }
    
}