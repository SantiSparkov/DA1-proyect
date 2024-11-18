using TaskPanelLibrary.Config;
using TaskPanelLibrary.DataTest;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelTest.ConfigTest;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.RepositoryTest.SqlRepositories;

[TestClass]
public class PanelSqlRepositoryTest
{
    private SqlContext _sqlContext;

    private IPanelRepository _panelRepository;
    
    [TestInitialize]
    public void Initialize()
    {
        _sqlContext = new SqlContexTest().CreateMemoryContext();
        _panelRepository = new PanelSqlRepository(_sqlContext);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext?.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void AddPanel()
    {
        //Arrange
        Panel panel = new Panel()
        {
                Id = 1,
                CreatorId = 1,
                Description = "Desc panel",
                Epicas = new List<Epic>(),
                IsDeleted = false,
                Name = "Name panel",
                Tasks = new List<Task>(),
                Team = new Team()
                {
                    Name = "Team 1",
                    TasksDescription = "Desc test",
                    MaxAmountOfMembers = 10
                }
            };
        
        //Act
        _panelRepository.AddPanel(panel);
        List<Panel> panels = _panelRepository.GetAllPanels();
        
        //Assert
        Assert.IsNotNull(panels);
        Assert.AreEqual(1, panels.Count());
    }
    
    [TestMethod]
    public void DeletePanel()
    {
        //Arrange
        Panel panel = new Panel()
        {
            Id = 1,
            CreatorId = 1,
            Description = "Desc panel",
            Epicas = new List<Epic>(),
            IsDeleted = false,
            Name = "Name panel",
            Tasks = new List<Task>(),
            Team = new Team()
            {
                Name = "Team 1",
                TasksDescription = "Desc test",
                MaxAmountOfMembers = 10
            }
        };
        
        //Act
        _panelRepository.AddPanel(panel);
        Panel panelDeleted = _panelRepository.DeletePanel(panel.Id);

        //Assert
        Assert.AreEqual(panel.Id, panelDeleted.Id);
        Assert.AreEqual(panel.Name, panelDeleted.Name);
        Assert.AreEqual(panel.CreatorId, panelDeleted.CreatorId);
        Assert.AreEqual(panel.IsDeleted, panelDeleted.IsDeleted);
        Assert.AreEqual(panel.Description, panelDeleted.Description);
        Assert.IsNotNull(panelDeleted.Tasks);
        Assert.IsNotNull(panelDeleted.Team);
        Assert.IsNotNull(panelDeleted.Epicas);
    }
    
    [TestMethod]
    public void DeletePanelNotExist()
    {
        //Arrange
        Panel panel = new Panel()
        {
            Id = 1,
            CreatorId = 1,
            Description = "Desc panel",
            Epicas = new List<Epic>(),
            IsDeleted = false,
            Name = "Name panel",
            Tasks = new List<Task>(),
            Team = new Team()
            {
                Name = "Team 1",
                TasksDescription = "Desc test",
                MaxAmountOfMembers = 10
            }
        };
        
        //Act 
        var exception = Assert.ThrowsException<System.Exception>(() => _panelRepository.DeletePanel(panel.Id));

        // Assert
        Assert.AreEqual($"Panel with id: {panel.Id} does not exist", exception.Message);
    }
    
    [TestMethod]
    public void GetPanelById()
    {
        //Arrange
        Panel panel = new Panel()
        {
            Id = 1,
            CreatorId = 1,
            Description = "Desc panel",
            Epicas = new List<Epic>(),
            IsDeleted = false,
            Name = "Name panel",
            Tasks = new List<Task>(),
            Team = new Team()
            {
                Name = "Team 1",
                TasksDescription = "Desc test",
                MaxAmountOfMembers = 10
            }
        };
        
        //Act
        _panelRepository.AddPanel(panel);
        Panel panelSaved = _panelRepository.GetPanelById(panel.Id);

        //Assert
        Assert.AreEqual(panel.Id, panelSaved.Id);
        Assert.AreEqual(panel.Name, panelSaved.Name);
        Assert.AreEqual(panel.CreatorId, panelSaved.CreatorId);
        Assert.AreEqual(panel.IsDeleted, panelSaved.IsDeleted);
        Assert.AreEqual(panel.Description, panelSaved.Description);
        Assert.IsNotNull(panelSaved.Tasks);
        Assert.IsNotNull(panelSaved.Team);
        Assert.IsNotNull(panelSaved.Epicas);
    }
    
    [TestMethod]
    public void GetPanelByIdNotExist()
    {
        //Arrange
        Panel panel = new Panel()
        {
            Id = 1,
            CreatorId = 1,
            Description = "Desc panel",
            Epicas = new List<Epic>(),
            IsDeleted = false,
            Name = "Name panel",
            Tasks = new List<Task>(),
            Team = new Team()
            {
                Name = "Team 1",
                TasksDescription = "Desc test",
                MaxAmountOfMembers = 10
            }
        };
        
        //Act 
        var exception = Assert.ThrowsException<System.Exception>(() => _panelRepository.GetPanelById(panel.Id));

        // Assert
        Assert.AreEqual($"Panel with id: {panel.Id} does not exist", exception.Message);
    }
    
    [TestMethod]
    public void UpdatePanel()
    {
        //Arrange
        Panel panel = new Panel()
        {
            Id = 1,
            CreatorId = 1,
            Description = "Desc panel",
            Epicas = new List<Epic>(),
            IsDeleted = false,
            Name = "Name panel",
            Tasks = new List<Task>(),
            Team = new Team()
            {
                Name = "Team 1",
                TasksDescription = "Desc test",
                MaxAmountOfMembers = 10
            }
        };
        
        //Act
        _panelRepository.AddPanel(panel);
        panel.Description = "Updated desc";
        panel.Name = "Name updated";
        panel.IsDeleted = true;
        Panel panelUpdated = _panelRepository.UpdatePanel(panel);

        //Assert
        Assert.AreEqual("Updated desc", panelUpdated.Description);
        Assert.AreEqual("Name updated", panelUpdated.Name);
        Assert.IsTrue(panelUpdated.IsDeleted);
    }
    
    [TestMethod]
    public void GetAllPanels()
    {
        //Arrange
        Panel panel = new Panel()
        {
            Id = 1,
            CreatorId = 1,
            Description = "Desc panel",
            Epicas = new List<Epic>(),
            IsDeleted = false,
            Name = "Name panel",
            Tasks = new List<Task>(),
            Team = new Team()
            {
                Name = "Team 1",
                TasksDescription = "Desc test",
                MaxAmountOfMembers = 10
            }
        };
        
        //Act
        _panelRepository.AddPanel(panel);
        List<Panel> panels = _panelRepository.GetAllPanels();

        //Assert
        Assert.AreEqual(1, panels.Count);
    }
    
    [TestMethod]
    public void Count()
    {
        //Arrange
        Panel panel = new Panel()
        {
            Id = 1,
            CreatorId = 1,
            Description = "Desc panel",
            Epicas = new List<Epic>(),
            IsDeleted = false,
            Name = "Name panel",
            Tasks = new List<Task>(),
            Team = new Team()
            {
                Name = "Team 1",
                TasksDescription = "Desc test",
                MaxAmountOfMembers = 10
            }
        };
        
        //Act
        _panelRepository.AddPanel(panel);

        //Assert
        Assert.AreEqual(1, _panelRepository.Count());
    }
}