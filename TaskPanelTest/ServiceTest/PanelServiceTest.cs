using Moq;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.Panel;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TaskPanelTest.ServiceTest
{

    [TestClass]
    public class PanelServiceTests
    {
        private Mock<IPanelRepository> _mockPanelRepository;
        private Mock<IUserService> _mockUserService;
        private Mock<ITrashService> _mockTrashService;
        private Mock<ITeamService> _mockTeamService;
        private PanelService _panelService;

        [TestInitialize]
        public void Setup()
        {
            _mockPanelRepository = new Mock<IPanelRepository>();
            _mockUserService = new Mock<IUserService>();
            _mockTrashService = new Mock<ITrashService>();
            _mockTeamService = new Mock<ITeamService>();

            _panelService = new PanelService(
                _mockPanelRepository.Object,
                _mockUserService.Object,
                _mockTrashService.Object,
                _mockTeamService.Object
            );
        }

        [TestMethod]
        public void CreatePanel_ShouldAddPanel_WhenPanelIsValid()
        {
            // Arrange
            var panel = new Panel
            {
                Name = "New Panel",
                Description = "Description of the panel",
                Team = new Team { Id = 1 }
            };
            var user = new User { Id = 1 };
            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);
            _mockPanelRepository.Setup(repo => repo.AddPanel(It.IsAny<Panel>())).Returns(panel);

            // Act
            var createdPanel = _panelService.CreatePanel(panel, user.Id);

            // Assert
            Assert.AreEqual(panel.Name, createdPanel.Name);
            Assert.AreEqual(panel.Description, createdPanel.Description);
            Assert.AreEqual(panel.Team.Id, createdPanel.Team.Id);
        }

        [TestMethod]
        public void CreatePanel_ShouldThrowException_WhenPanelNameIsEmpty()
        {
            // Arrange
            var team = new Team { Id = 1 };

            var panel = new Panel { Name = "", Description = "", Team = team };

            // Act & Assert
            Assert.ThrowsException<PanelNotValidException>(() => _panelService.CreatePanel(panel, 1));
        }

        [TestMethod]
        public void CreatePanel_ShouldThrowException_WhenPanelIsNull()
        {
            Panel panel = null;
            User user = new User { Id = 1 };
            
            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);
            
            // Act & Assert
            Assert.ThrowsException<PanelNotValidException>(() => _panelService.CreatePanel(panel, user.Id));
        }
        
        [TestMethod]
        public void CreatePanel_ShouldThrowException_WhenPanelDescriptionIsEmpty()
        {

            // Arrange
            var team = new Team { Id = 1 };

            var panel = new Panel { Name = "Panel", Description = "", Team = team };

            // Act & Assert
            Assert.ThrowsException<PanelNotValidException>(() => _panelService.CreatePanel(panel, 1));
        }

        
        [TestMethod]
        public void CreatePanel_ShouldThrowException_WhenPanelTeamIsNull()
        {
            // Arrange
            var panel = new Panel { Name = "Panel", Description = "", Team = null };

            // Act & Assert
            Assert.ThrowsException<PanelNotValidException>(() => _panelService.CreatePanel(panel, 1));
        }

        [TestMethod]
        public void GetAllPanelForTeam_ShouldReturnPanelsForTeam()
        {
            // Arrange
            var teamId = 1;
            var panels = new List<Panel>
            {
                new Panel { Team = new Team { Id = teamId } },
                new Panel { Team = new Team { Id = 2 } }
            };
            _mockPanelRepository.Setup(repo => repo.GetAllPanels()).Returns(panels);

            // Act
            var result = _panelService.GetAllPanelForTeam(teamId);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(teamId, result[0].Team.Id);
        }

        [TestMethod]
        public void GetAllPanelForUser_ShouldReturnPanelsForUser()
        {
            // Arrange
            var userId = 1;
            var team = new Team { Id = 1, Panels = new List<Panel> { new Panel { Team = new Team { Id = 1 } } } };
            _mockTeamService.Setup(service => service.TeamsForUser(It.IsAny<int>())).Returns(new List<Team> { team });

            // Act
            var result = _panelService.GetAllPanelForUser(userId);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(team.Id, result[0].Team.Id);
        }

        [TestMethod]
        public void UpdatePanel_ShouldUpdatePanel()
        {
            // Arrange
            var panel = new Panel { Id = 1, Name = "Panel 1" };
            _mockPanelRepository.Setup(repo => repo.GetPanelById(It.IsAny<int>())).Returns(panel);
            _mockPanelRepository.Setup(repo => repo.UpdatePanel(It.IsAny<Panel>())).Returns(panel);

            // Act
            var updatedPanel = _panelService.UpdatePanel(panel);

            // Assert
            Assert.AreEqual(panel.Name, updatedPanel.Name);
        }

        [TestMethod]
        public void DeletePanel_ShouldMarkAsDeleted_WhenPanelExists()
        {
            // Arrange
            var panel = new Panel { Id = 1, IsDeleted = true };
            var user = new User { Id = 1, IsAdmin = true, TrashId = 123 };
            _mockPanelRepository.Setup(repo => repo.GetPanelById(It.IsAny<int>())).Returns(panel);
            _mockTrashService.Setup(service => service.AddPanelToTrash(It.IsAny<Panel>(), It.IsAny<int>()));

            // Act
            var deletedPanel = _panelService.DeletePanel(panel.Id, user);
            Assert.IsTrue(deletedPanel.IsDeleted);
        }
        
        [TestMethod]
        public void DeletePanel_NotDeletedPanel_ShouldDeletePanel()
        {
            // Arrange
            var panel = new Panel { Id = 1, IsDeleted = false };
            var user = new User { Id = 1, IsAdmin = true, TrashId = 123 };
            _mockPanelRepository.Setup(repo => repo.GetPanelById(It.IsAny<int>())).Returns(panel);
            _mockTrashService.Setup(service => service.RemovePanelFromTrash(It.IsAny<int>(), It.IsAny<int>()));
            _mockPanelRepository.Setup(repo => repo.DeletePanel(It.IsAny<int>()));
            
            // Act
            var deletedPanel = _panelService.DeletePanel(panel.Id, user);
            
            // Assert
            Assert.IsTrue(deletedPanel.IsDeleted);
        }
        
        [TestMethod]
        public void DeletePanel_NonAdminUser_ShouldThrowException()
        {
            // Arrange
            var panel = new Panel { Id = 1, IsDeleted = false };
            var user = new User { Id = 1, IsAdmin = false, TrashId = 123 };
            _mockPanelRepository.Setup(repo => repo.GetPanelById(It.IsAny<int>())).Returns(panel);

            // Act & Assert
            Assert.ThrowsException<PanelNotValidException>(() => _panelService.DeletePanel(panel.Id, user));
        }
        
        [TestMethod]
        public void RestorePanel_NonAdminUser_ShouldThrowException()
        {
            // Arrange
            var panel = new Panel { Id = 1, IsDeleted = true };
            var user = new User { Id = 1, IsAdmin = false, TrashId = 123 };
            _mockPanelRepository.Setup(repo => repo.GetPanelById(It.IsAny<int>())).Returns(panel);

            // Act & Assert
            Assert.ThrowsException<PanelNotValidException>(() => _panelService.RestorePanel(panel.Id, user));
        }

        [TestMethod]
        public void RestorePanel_ShouldRestoreDeletedPanel()
        {
            // Arrange
            var panel = new Panel { Id = 1, IsDeleted = true };
            var user = new User { Id = 1, IsAdmin = true, TrashId = 123 };
            _mockPanelRepository.Setup(repo => repo.GetPanelById(It.IsAny<int>())).Returns(panel);
            _mockTrashService.Setup(service => service.GetTrashById(It.IsAny<int>())).Returns(new Trash
            {
                PanelList = new List<Panel> { panel }
            });

            // Act
            var restoredPanel = _panelService.RestorePanel(panel.Id, user);

            // Assert
            Assert.IsFalse(restoredPanel.IsDeleted);
        }

        [TestMethod]
        public void GetPanelById_ShouldReturnPanel()
        {
            // Arrange
            var panel = new Panel { Id = 1 };
            _mockPanelRepository.Setup(repo => repo.GetPanelById(It.IsAny<int>())).Returns(panel);

            // Act
            var result = _panelService.GetPanelById(1);

            // Assert
            Assert.AreEqual(panel.Id, result.Id);
        }


        [TestMethod]
        public void GetAllPanels_ShouldReturnAllPanels()
        {
            // Arrange
            var panels = new List<Panel>
            {
                new Panel { Id = 1 },
                new Panel { Id = 2 }
            };
            _mockPanelRepository.Setup(repo => repo.GetAllPanels()).Returns(panels);

            // Act
            var result = _panelService.GetAllPanels();

            // Assert
            Assert.AreEqual(2, result.Count);
        }
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