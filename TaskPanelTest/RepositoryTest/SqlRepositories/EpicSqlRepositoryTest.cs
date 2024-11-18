using TaskPanelLibrary.Config;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelTest.ConfigTest;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.RepositoryTest.SqlRepositories;

[TestClass]
public class EpicSqlRepositoryTest
{
    private SqlContext _sqlContext;

    private IEpicRepository _epicRepository;
    
    [TestInitialize]
    public void Initialize()
    {
        _sqlContext = new SqlContexTest().CreateMemoryContext();
        _epicRepository = new EpicSqlRepository(_sqlContext);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext?.Database.EnsureDeleted();
    }

    [TestMethod]
    public void AddEpic()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Title = "Title test",
            Id = 1,
            Description = "Test desc",
            DueDateTime = new DateTime(2024, 11, 17),
            PanelId = 1,
            Priority = EPriority.MEDIUM,
            Tasks = new List<Task>()
        };
        
        //Act
        _epicRepository.AddEpic(epic);
        List<Epic> epics = _epicRepository.GetAllEpics();
        //Assert
        
        Assert.AreEqual(1, epics.Count);
    }
    
    [TestMethod]
    public void DeleteEpic()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Title = "Title test",
            Id = 1,
            Description = "Test desc",
            DueDateTime = new DateTime(2024, 11, 17),
            PanelId = 1,
            Priority = EPriority.MEDIUM,
            Tasks = new List<Task>()
        };
        
        //Act
        _epicRepository.AddEpic(epic);
        Epic epicDeleted = _epicRepository.DeleteEpic(epic.Id);
        
        //Assert
        Assert.AreEqual(epic.Id, epicDeleted.Id);
        Assert.AreEqual(epic.Title, epicDeleted.Title);
        Assert.AreEqual(epic.PanelId, epicDeleted.PanelId);
        Assert.AreEqual(epic.Priority, epicDeleted.Priority);
        Assert.AreEqual(epic.Description, epicDeleted.Description);
        Assert.IsNotNull(epicDeleted.Tasks);
        Assert.AreEqual(epic.DueDateTime, epicDeleted.DueDateTime);
    }
    
    [TestMethod]
    public void DeleteEpicNotExist()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Title = "Title test",
            Id = 1,
            Description = "Test desc",
            DueDateTime = new DateTime(2024, 11, 17),
            PanelId = 1,
            Priority = EPriority.MEDIUM,
            Tasks = new List<Task>()
        };
        
        //Act 
        var exception = Assert.ThrowsException<EpicNotValidException>(() => _epicRepository.DeleteEpic(epic.Id));

        // Assert
        Assert.AreEqual($"Epic with id: {epic.Id} does not exist", exception.Message);
    }
    
    [TestMethod]
    public void GetEpicById()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Title = "Title test",
            Id = 1,
            Description = "Test desc",
            DueDateTime = new DateTime(2024, 11, 17),
            PanelId = 1,
            Priority = EPriority.MEDIUM,
            Tasks = new List<Task>()
        };
        
        //Act
        _epicRepository.AddEpic(epic);
        Epic epicSaved = _epicRepository.GetEpicById(epic.Id);
        
        //Assert
        Assert.AreEqual(epic.Id, epicSaved.Id);
        Assert.AreEqual(epic.Title, epicSaved.Title);
        Assert.AreEqual(epic.PanelId, epicSaved.PanelId);
        Assert.AreEqual(epic.Priority, epicSaved.Priority);
        Assert.AreEqual(epic.Description, epicSaved.Description);
        Assert.IsNotNull(epicSaved.Tasks);
        Assert.AreEqual(epic.DueDateTime, epicSaved.DueDateTime);
    }
    
    [TestMethod]
    public void GetEpicByIdNotExist()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Title = "Title test",
            Id = 1,
            Description = "Test desc",
            DueDateTime = new DateTime(2024, 11, 17),
            PanelId = 1,
            Priority = EPriority.MEDIUM,
            Tasks = new List<Task>()
        };
        
        //Act 
        var exception = Assert.ThrowsException<EpicNotValidException>(() => _epicRepository.GetEpicById(epic.Id));

        // Assert
        Assert.AreEqual($"Epic with id: {epic.Id} does not exist", exception.Message);
    }
    
    [TestMethod]
    public void GetAllEpics()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Title = "Title test",
            Id = 1,
            Description = "Test desc",
            DueDateTime = new DateTime(2024, 11, 17),
            PanelId = 1,
            Priority = EPriority.MEDIUM,
            Tasks = new List<Task>()
        };
        
        Epic epic2 = new Epic()
        {
            Title = "Title test2",
            Id = 2,
            Description = "Test desc2",
            DueDateTime = new DateTime(2024, 11, 17),
            PanelId = 1,
            Priority = EPriority.LOW,
            Tasks = new List<Task>()
        };
        
        //Act
        _epicRepository.AddEpic(epic);
        _epicRepository.AddEpic(epic2);
        
        List<Epic> epics = _epicRepository.GetAllEpics();
        
        //Assert
        Assert.AreEqual(2, epics.Count);
    }
    
    [TestMethod]
    public void UpdateEpic()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Title = "Title test",
            Id = 1,
            Description = "Test desc",
            DueDateTime = new DateTime(2024, 11, 17),
            PanelId = 1,
            Priority = EPriority.MEDIUM,
            Tasks = new List<Task>()
        };
        
        //Act
        _epicRepository.AddEpic(epic);

        epic.Description = "Update";
        epic.Priority = EPriority.HIGH;
        epic.DueDateTime = new DateTime(2024, 12, 11);
        epic.Title = "Title updated";
        
        Epic epicUpdated = _epicRepository.UpdateEpic(epic);
        
        //Assert
        Assert.AreEqual("Title updated", epicUpdated.Title);
        Assert.AreEqual(EPriority.HIGH, epicUpdated.Priority);
        Assert.AreEqual("Update", epicUpdated.Description);
        Assert.AreEqual(new DateTime(2024, 12, 11), epicUpdated.DueDateTime);
    }
    
    [TestMethod]
    public void Count()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Title = "Title test",
            Id = 1,
            Description = "Test desc",
            DueDateTime = new DateTime(2024, 11, 17),
            PanelId = 1,
            Priority = EPriority.MEDIUM,
            Tasks = new List<Task>()
        };
        
        //Act
        _epicRepository.AddEpic(epic);
        
        //Assert
        Assert.AreEqual(1, _epicRepository.Count());
    }
}