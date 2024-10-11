using Microsoft.AspNetCore.Identity;
using Moq;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.ServiceTest;

[TestClass]
public class PanelServiceTest
{
    private PanelService _panelService;

    private PanelRepository panelRepository;

    private TaskService _taskService;
    
    private Panel panel;

    private User user;
    
    private TaskRepository taskRepository;
    
    private CommentService _commentService;

    private CommentRepository _commentRepository;


    [TestInitialize]
    public void Initialize()
    {
        //Arrange
        _commentService = new CommentService(_commentRepository);
        taskRepository = new TaskRepository();
        _panelService = new PanelService(panelRepository, _taskService);
        panelRepository = new PanelRepository();
        _taskService = new TaskService(taskRepository, _panelService, _commentService);
        _panelService = new PanelService(panelRepository, _taskService);
        
        user = new User()
        {
            Name = "User Manager",
            Id = 1,
            IsAdmin = true,
            Email = "prueba@hotmail.com"
        };
    }

    [TestMethod]
    public void createPanelService()
    {
        //Arrange 
        //Act 
        // Assert
        Assert.IsNotNull(_panelService);
    }
    
    [TestMethod]
    public void AddTask()
    {
        //Arrange
        Panel panel = _panelService.CreatePanel(user);
        Task task = new Task()
        {
            Title = "Test",
            Description = "Desc test"
        };
        
        //Act 
        var result = _panelService.AddTask(panel.Id, task);

        
        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual(result, task);
        Assert.IsNotNull(panel.Tasks.Contains(task));
    }
    
    [TestMethod]
    public void DeleteTask()
    {
        //Arrange 
        Task task = new Task()
        {
            Title = "Test",
            Description = "Desc test"
        };
        
        Panel panel = _panelService.CreatePanel(user);
        
        //Act
        Task result = _panelService.AddTask(panel.Id, task);
        Task taskDelete = _panelService.DeleteTask(task, user);
        
        // Assert
        Assert.IsNotNull(result);
        Assert.IsTrue(user.Trash.TaskList.Contains(task));
        Assert.IsNotNull(taskDelete);
        Assert.AreEqual(taskDelete, task);
        Assert.AreEqual(taskDelete, result);
    }

    [TestMethod] public void DeletePanel()
    {
        //Arrange 
        Panel panel = _panelService.CreatePanel(user);
        
        //Act 
        Panel panelDeleted = _panelService.DeletePanel(panel.Id, user);
        var exception = Assert.ThrowsException<System.ArgumentException>(() => _panelService.FindById(panelDeleted.Id));

        // Assert
        Assert.IsTrue(user.Trash.PanelList.Contains(panelDeleted));
        Assert.AreEqual(exception.Message, "Panel does not exist");

    }
    
    [TestMethod]
    public void DeletePanelNotAdmin()
    {
        //Arrange 
        Panel panel = _panelService.CreatePanel(user);
        User user1 = new User()
        {
            Id = 1,
            IsAdmin = false
        };
        //Act 
        var exception = Assert.ThrowsException<TaskPanelException>(() => _panelService.DeletePanel(panel.Id, user1));

        // Assert
        Assert.AreEqual(exception.Message, $"User is not admin, userId: {user1.Id}");
    }
    
    [TestMethod]
    public void UpdatePanel()
    {
        //Arrange 
        Panel panel = _panelService.CreatePanel(user);

        Panel newPanel = new Panel()
        {
            Name = "Panel actualziado",
            Description = "Descripción Actualziada",
        };

        _panelService.UpdatePanel(newPanel);
        Panel panelUpdated = _panelService.FindById(panel.Id);
        //Act 
        // Assert
        Assert.IsNotNull(panelUpdated);
        Assert.AreEqual(newPanel.Description, panelUpdated.Description);
        Assert.AreEqual(newPanel.Name, panelUpdated.Name);
    }
    
    [TestMethod]
    public void AddTeamPanel()
    {
        //Arrange 
        User user1 = new User()
        {
            Id = 1,
            IsAdmin = true
        };
        User user2 = new User()
        {
            Id = 2,
            IsAdmin = false
        };
        User user3 = new User()
        {
            Id = 3,
            IsAdmin = false
        };
        User user4 = new User()
        {
            Id = 4,
            IsAdmin = false
        };
        List<User> users = new List<User>();
        users.Add(user1);
        users.Add(user2);
        users.Add(user3);
        users.Add(user4);
        Panel panel = _panelService.CreatePanel(user1);
        Team team = new Team()
        {
            Id = 123,
            Users = users
        };
        //Act 
        _panelService.AddTeam(panel.Id, team);
        Panel panelWithTeam = _panelService.FindById(panel.Id);

        // Assert
        Assert.IsNotNull(team);
        Assert.AreEqual(4,panelWithTeam.Team.Users.Count);
    }
    
    [TestMethod]
    public void AddTeamWithUserRepeatedPanel()
    {
        //Arrange 
        User user1 = new User()
        {
            Id = 1,
            IsAdmin = true
        };
        User user2 = new User()
        {
            Id = 2,
            IsAdmin = false
        };
        List<User> users = new List<User>();
        users.Add(user1);
        users.Add(user2);
        users.Add(user2);
        users.Add(user2);
        Panel panel = _panelService.CreatePanel(user1);
        Team team = new Team()
        {
            Id = 123,
            Users = users
        };
        //Act 
        _panelService.AddTeam(panel.Id, team);
        Panel panelWithTeam = _panelService.FindById(panel.Id);

        // Assert
        Assert.AreEqual(2,panelWithTeam.Team.Users.Count);
    }
    
    [TestMethod]
    public void AddUserTeam()
    {
        //Arrange 
        Panel panel = _panelService.CreatePanel(user);
        User user1 = new User()
        {
            Id = 1,
            IsAdmin = true
        };

        //Act 
        _panelService.AddUser(panel.Id, user1);
        Team team = _panelService.FindById(panel.Id).Team;
        User userSaved = team.Users.Find(u => u.Id == user1.Id);
        
        // Assert
        Assert.AreEqual(userSaved.Id, user1.Id);
        Assert.AreEqual(userSaved.IsAdmin, user1.IsAdmin);
        Assert.AreEqual(2,panel.Team.Users.Count);
    }
    
    [TestMethod]
    public void RemoveUserTeam()
    {
        //Arrange 
        Panel panel = _panelService.CreatePanel(user);
        User user1 = new User()
        {
            Id = 1,
            IsAdmin = true
        };
        _panelService.AddUser(panel.Id, user1);
        //Act 
        User userDeleted = _panelService.RemoveUser(panel.Id, user1);
        
        // Assert
        Assert.AreEqual(1,panel.Team.Users.Count);
    }
    
    [TestMethod]
    public void RemoveUserNotExistInGroup()
    {
        //Arrange 
        Panel panel = _panelService.CreatePanel(user);
        User user1 = new User()
        {
            Id = 1,
            IsAdmin = true
        };
        //Act 
        var exception = Assert.ThrowsException<TaskPanelException>(() => _panelService.RemoveUser(panel.Id, user1));

        // Assert
        Assert.AreEqual(exception.Message, $"User does not belong to the group, userId: {user1.Id}");
    }
    
    [TestCleanup]
    public void Cleanup()
    {
        panelRepository.GetAll().Clear();
    }
    
}