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
    
    private IPanelService _panelService;
    
    private IUserService _userService;
    
    private PasswordGeneratorService _passwordGeneratorService;

    private Panel _panel;

    private User _user;
    
    [TestInitialize]
    public void Initialize()
    {
        _panelRepository = new PanelRepository();
        _userRepository = new UserRepository();
        
        _passwordGeneratorService = new PasswordGeneratorService();
        _userService = new UserService(_userRepository, _passwordGeneratorService);
        _panelService = new PanelService(_panelRepository, _userService);
        
        _user = new User()
        {
            Name = "User Manager",
            Id = 1,
            IsAdmin = true,
            Email = "prueba@hotmail.com"
        };
        
        _panel = new Panel()
        {
            Name = "Panel Test",
            Team = new Team(){Id = 1, Name = "Team Test"},
            Description = "Description Test",
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
}