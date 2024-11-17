using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using TaskPanelLibrary.Config;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.ServiceTest;

[TestClass]
public class EpicServiceTest
{
    private IEpicService _epicService;

    private Mock<IEpicRepository> _epicRepository;
    
    private Mock<IPanelService> _panelService;
    

    [TestInitialize]
    public void SetUp()
    {
        _epicRepository = new Mock<IEpicRepository>();
        _panelService = new Mock<IPanelService>();
        _epicService = new EpicService(_epicRepository.Object, _panelService.Object);
    }

    [TestCleanup]
    public void CleanUp()
    {
        
    }
    
    [TestMethod]
    public void CreateEpic()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Description = "Epic test",
            Title = "test",
            DueDateTime = new DateTime(2024, 12, 12),
            PanelId = 1,
            Priority = EPriority.LOW,
            Tasks = new List<Task>()
        };

        Panel panel = new Panel()
        {
            Id = 1
        };
        
        _epicRepository.Setup(service => service.AddEpic(It.IsAny<Epic>()))
            .Returns((epic));
        
        _panelService.Setup(service => service.GetPanelById(It.IsAny<int>()))
            .Returns(panel);
        
        //Act
        Epic epicCreate = _epicService.CreateEpic(epic, 1);
        
        Assert.AreEqual(epicCreate.Description, epic.Description);
        Assert.AreEqual(epicCreate.DueDateTime, epic.DueDateTime);
        Assert.AreEqual(epicCreate.PanelId, epic.PanelId);
        Assert.AreEqual(epicCreate.Priority, epic.Priority);
        Assert.IsNotNull(epicCreate.Tasks);
    }
    
    [TestMethod]
    public void CreateEpicTitleInvalid()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Description = "Epic test",
            DueDateTime = new DateTime(2024, 12, 12),
            PanelId = 1,
            Priority = EPriority.LOW,
            Tasks = new List<Task>()
        };

        Panel panel = new Panel()
        {
            Id = 1
        };
        
        _epicRepository.Setup(service => service.AddEpic(It.IsAny<Epic>()))
            .Returns((epic));
        
        _panelService.Setup(service => service.GetPanelById(It.IsAny<int>()))
            .Returns(panel);
        
        //Act
        var exception = Assert.ThrowsException<EpicNotValidException>(() => _epicService.CreateEpic(epic, 1));
        Assert.AreEqual(exception.Message, "Epic title is null or empty");
    }

    [TestMethod]
    public void CreateEpicDescriptionInvalid()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Title = "Epic test",
            DueDateTime = new DateTime(2024, 12, 12),
            PanelId = 1,
            Priority = EPriority.LOW,
            Tasks = new List<Task>()
        };

        Panel panel = new Panel()
        {
            Id = 1
        };

        _epicRepository.Setup(service => service.AddEpic(It.IsAny<Epic>()))
            .Returns((epic));

        _panelService.Setup(service => service.GetPanelById(It.IsAny<int>()))
            .Returns(panel);

        //Act
        var exception = Assert.ThrowsException<EpicNotValidException>(() => _epicService.CreateEpic(epic, 1));
        Assert.AreEqual(exception.Message, "Epic description is null or empty");
    }

    [TestMethod]
    public void CreateEpicNull()
    {
        //Arrange
        Epic epic = null;

        Panel panel = new Panel()
        {
            Id = 1
        };
        
        _epicRepository.Setup(service => service.AddEpic(It.IsAny<Epic>()))
            .Returns((epic));
        
        _panelService.Setup(service => service.GetPanelById(It.IsAny<int>()))
            .Returns(panel);
        
        //Act
        var exception = Assert.ThrowsException<EpicNotValidException>(() => _epicService.CreateEpic(epic, 1));
        Assert.AreEqual(exception.Message, "Epic is null");
    }
    
    
    [TestMethod]
    public void GetEpicById()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Description = "Epic test",
            DueDateTime = new DateTime(2024, 12, 12),
            PanelId = 1,
            Priority = EPriority.LOW,
            Tasks = new List<Task>()
        };
        
        _epicRepository.Setup(service => service.GetEpicById(It.IsAny<int>()))
            .Returns(epic);
        //Act
        Epic epicCreate = _epicService.GetEpicById(1);
        
        Assert.AreEqual(epicCreate.Description, epic.Description);
        Assert.AreEqual(epicCreate.DueDateTime, epic.DueDateTime);
        Assert.AreEqual(epicCreate.PanelId, epic.PanelId);
        Assert.AreEqual(epicCreate.Priority, epic.Priority);
        Assert.IsNotNull(epicCreate.Tasks);
    }
    
    [TestMethod]
    public void GetAllEpicsByPanelId()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Description = "Epic test",
            DueDateTime = new DateTime(2024, 12, 12),
            PanelId = 1,
            Priority = EPriority.LOW,
            Tasks = new List<Task>()
        };
        List<Epic> epicsForPanel = new List<Epic>();
        epicsForPanel.Add(epic);
        
        _epicRepository.Setup(service => service.GetAllEpics())
            .Returns(epicsForPanel);

        //Act
        List<Epic> epicsByPanel= _epicService.GetAllEpicsByPanelId(1);
        Epic epicCreate = epicsByPanel[0];
        
        Assert.AreEqual(1, epicsByPanel.Count);
        Assert.AreEqual(epicCreate.Description, epic.Description);
        Assert.AreEqual(epicCreate.DueDateTime, epic.DueDateTime);
        Assert.AreEqual(epicCreate.PanelId, epic.PanelId);
        Assert.AreEqual(epicCreate.Priority, epic.Priority);
        Assert.IsNotNull(epicCreate.Tasks);
    }

   [TestMethod]
    public void UpdateEpic()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Description = "Epic test 1",
            DueDateTime = new DateTime(2023, 12, 12),
            Title = "Title 1",
            PanelId = 1,
            Priority = EPriority.LOW,
            Tasks = new List<Task>()
        };

        Panel panel = new Panel()
        {
            Id = 1
        };
        
        _epicRepository.Setup(service => service.AddEpic(It.IsAny<Epic>()))
            .Returns((epic));
        
        _epicRepository.Setup(service => service.GetEpicById(It.IsAny<int>()))
            .Returns(epic);

        _epicRepository.Setup(service => service.UpdateEpic(It.IsAny<Epic>()))
            .Returns(epic);
        
        _panelService.Setup(service => service.GetPanelById(It.IsAny<int>()))
            .Returns(panel);

        //Act
        Epic epicCreate = _epicService.CreateEpic(epic, 1);
        epic.Title = "Title 2";
        epic.Description = "Desc test 2";
        epic.DueDateTime = new DateTime(2024, 03, 09);
        epic.Priority = EPriority.HIGH;

        Epic epicSaved = _epicService.UpdateEpic(epic);

        Assert.AreEqual("Title 2", epicSaved.Title);
        Assert.AreEqual("Desc test 2", epicSaved.Description);
        Assert.AreEqual(new DateTime(2024, 03, 09), epicSaved.DueDateTime);
        Assert.AreEqual(1, epicSaved.PanelId);
        Assert.AreEqual(EPriority.HIGH, epicSaved.Priority);
        Assert.IsNotNull(epicSaved.Tasks);
    }
    
    [TestMethod]
    public void DeleteEpic()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Title = "Title 1",
            Description = "Epic test 1",
            DueDateTime = new DateTime(2023, 12, 12),
            PanelId = 1,
            Priority = EPriority.LOW,
            Tasks = new List<Task>()
        };
        
        
        _epicRepository.Setup(service => service.GetEpicById(It.IsAny<int>()))
            .Returns(epic);
        
        _epicRepository.Setup(service => service.DeleteEpic(It.IsAny<int>()))
            .Returns(epic);

        //Act
        Epic epicDeleted = _epicService.DeleteEpic(epic.Id);

        Assert.AreEqual("Title 1", epicDeleted.Title);
        Assert.AreEqual("Epic test 1", epicDeleted.Description);
        Assert.AreEqual(new DateTime(2023, 12, 12), epicDeleted.DueDateTime);
        Assert.AreEqual(1, epicDeleted.PanelId);
        Assert.AreEqual(EPriority.LOW, epicDeleted.Priority);
        Assert.IsNotNull(epicDeleted.Tasks);
    }
    
    [TestMethod]
    public void DeleteEpicException()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Title = "Title 1",
            Description = "Epic test 1",
            DueDateTime = new DateTime(2023, 12, 12),
            PanelId = 1,
            Priority = EPriority.LOW,
            Tasks = new List<Task>()
        };

        Task task = new Task()
        {
            Id = 1
        };
        
        epic.Tasks.Add(task);
        
        _epicRepository.Setup(service => service.GetEpicById(It.IsAny<int>()))
            .Returns(epic);
        
        _epicRepository.Setup(service => service.DeleteEpic(It.IsAny<int>()))
            .Returns(epic);
        
        //Act
        var exception = Assert.ThrowsException<EpicNotValidException>(() => _epicService.DeleteEpic(epic.Id));
        Assert.AreEqual(exception.Message, "Epic has tasks");
    }
    
    [TestMethod]
    public void GetTasksFromEpic()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Title = "Title 1",
            Description = "Epic test 1",
            DueDateTime = new DateTime(2023, 12, 12),
            PanelId = 1,
            Priority = EPriority.LOW,
            Tasks = new List<Task>()
        };

        Task task = new Task()
        {
            Id = 1
        };
        
        epic.Tasks.Add(task);
        
        
        _epicRepository.Setup(service => service.GetEpicById(It.IsAny<int>()))
            .Returns(epic);
        

        //Act
        List<Task> tasks = _epicService.GetTasksFromEpic(epic.Id);

        Assert.AreEqual(1, tasks.Count);
        Assert.AreEqual(1, tasks[0].Id);
        Assert.IsNotNull(tasks);
    }
}