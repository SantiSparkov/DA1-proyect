using Microsoft.AspNetCore.Identity;
using Moq;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Exception.Panel;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.ServiceTest;

[TestClass]
public class PanelServiceTest
{
    private IUserRepository _userRepository;
    
    private IPanelRepository _panelRepository;
    
    private ITeamRepository _teamRepository;
    
    private IPanelService _panelService;
    
    private IUserService _userService;
    
    private ITeamService _teamService;
    
    private PasswordGeneratorService _passwordGeneratorService;

    private Panel _panel;

    private User _user;

    private Team _team;
    
    [TestInitialize]
    public void Initialize()
    {
        _panelRepository = new PanelRepository();
        _userRepository = new UserRepository();
        _teamRepository = new TeamRepository();
        _teamRepository = new TeamRepository();
        
        _passwordGeneratorService = new PasswordGeneratorService();
        _userService = new UserService(_userRepository, _passwordGeneratorService);
        _panelService = new PanelService(_panelRepository, _userService);
        _teamService = new TeamService(_teamRepository, _userService, _panelService);
        
        _user = new User()
        {
            Name = "User Manager",
            Id = 1,
            IsAdmin = true,
            Email = "prueba@hotmail.com",
            LastName = "Manager",
            BirthDate = new DateTime(1999, 10, 10),
        };
        
        _panel = new Panel()
        {
            Name = "Panel Test",
            Description = "Description Test",
        };
        
        _team = new Team()
        {
            Id = 1,
            Name = "Team L",
            TeamLeader = _user,
            MaxAmountOfMembers = 5,
            TasksDescription = "Task description for Team L"
        };
    }

    [TestMethod]
    public void createPanelService()
    {
        //Arrange 
        //Act 
        Panel panel = _panelService.CreatePanel(_panel);
        
        // Assert
        Assert.IsNotNull(panel);
        Assert.AreEqual(panel.Name, _panel.Name);
        Assert.AreEqual(panel.Description, _panel.Description);
    }

    [TestMethod]
    public void CreatePanel()
    {
        //Arrange 
        //Act 
        Panel panel = _panelService.CreatePanel(_panel);
        
        // Assert
        Assert.IsNotNull(panel);
        Assert.AreEqual(panel.Name, _panel.Name);
        Assert.AreEqual(panel.Description, _panel.Description);
    }

    [TestMethod]
    public void UpdatePanel_Valid()
    {
        //Arrange 
        Panel panel = _panelService.CreatePanel(_panel);
        panel.Name = "Panel Test Updated";
        panel.Description = "Description Test Updated";
        
        //Act 
        Panel panelUpdated = _panelService.UpdatePanel(panel);
        
        // Assert
        Assert.IsNotNull(panelUpdated);
        Assert.AreEqual(panelUpdated.Name, panel.Name);
        Assert.AreEqual(panelUpdated.Description, panel.Description);
    }
    
    [TestMethod]
    public void UpdatePanel_Invalid()
    {
        //Arrange 
        Panel panel = _panelService.CreatePanel(_panel);
        panel.Name = "Panel Test Updated";
        panel.Description = "Description Test Updated";
        
        //Act 
        Panel panelUpdated = _panelService.UpdatePanel(panel);
        
        // Assert
        Assert.IsNotNull(panelUpdated);
        Assert.AreEqual(panelUpdated.Name, panel.Name);
        Assert.AreEqual(panelUpdated.Description, panel.Description);
    }
    
    [TestMethod]
    public void DeletePanel_Valid()
    {
        //Arrange 
        Panel panel = _panelService.CreatePanel(_panel);
        
        //Act 
        _panelService.DeletePanel(panel.Id, _user);
        
        // Assert
        Assert.AreEqual(_panelRepository.GetAll().Count, 0);
    }
    
    [TestMethod]
    public void DeletePanel_Invalid()
    {
        //Arrange 
        Panel panel = _panelService.CreatePanel(_panel);
        
        //Act && Assert 
        Assert.ThrowsException<PanelNotValidException>(() => _panelService.DeletePanel(panel.Id, new User()));
    }
}