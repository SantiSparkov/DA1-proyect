using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.Team;
using TaskPanelLibrary.Exception.User;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;

namespace TaskPanelTest.ServiceTest
{
    [TestClass]
    public class TeamServiceTest
    {
        private ITeamService _teamService;
        
        private ITeamRepository _teamRepository;
        
        private IUserRepository _userRepository;
        
        private IPanelRepository _panelRepository;
        
        private IPanelService _panelService;
        
        private IUserService _userService;
        
        private PasswordGeneratorService _passwordGeneratorService;
        
        private User _adminUser;

        [TestInitialize]
        public void Initialize()
        {
            _teamRepository = new TeamRepository();
            _userRepository = new UserRepository();
            _panelRepository = new PanelRepository();
            
            _passwordGeneratorService = new PasswordGeneratorService();
            _userService = new UserService(_userRepository, _passwordGeneratorService);
            _panelService = new PanelService(_panelRepository, _userService);
            _teamService = new TeamService(_teamRepository, _userService, _panelService);
            
            _adminUser = new User {Name = "Admin User", IsAdmin = true, Email = "userAdministrator@gmail.com", LastName = "LasatMan"};
        }

        [TestMethod]
        public void CreateTeam_ValidTeam_CreatesTeamSuccessfully()
        {
            // Arrange
            _userService.AddUser(_adminUser);

            var team = new Team
            {
                Name = "Team A",
                TasksDescription = "Task description for Team A",
                MaxAmountOfMembers = 5
            };

            // Act
            var createdTeam = _teamService.CreateTeam(team, _adminUser.Id);

            // Assert
            Assert.IsNotNull(createdTeam);
            Assert.AreEqual(team.Name, createdTeam.Name);
            Assert.AreEqual(_adminUser.Id, createdTeam.TeamLeader.Id);
            Assert.AreEqual(1, createdTeam.Users.Count);
        }

        [TestMethod]
        public void CreateTeam_NonAdminUser_ThrowsException()
        {
            // Arrange
            var nonAdminUser = new User { Id = 2, Name = "New User", LastName = "User LastName",IsAdmin = false , Email = "user2@gmail.com"};
            _userService.AddUser(nonAdminUser);

            var team = new Team
            {
                Name = "Team B",
                TasksDescription = "Task description for Team B",
                MaxAmountOfMembers = 5
            };

            // Act & Assert
            Assert.ThrowsException<UserNotValidException>(() => _teamService.CreateTeam(team, nonAdminUser.Id));
        }

        [TestMethod]
        public void DeleteTeam_AdminUser_DeletesTeamSuccessfully()
        {
            // Arrange
            _userService.AddUser(_adminUser);

            var team = new Team
            {
                Id = 1,
                Name = "Team C",
                TeamLeader = _adminUser,
                MaxAmountOfMembers = 5,
                TasksDescription = "Task description for Team C",
                Panels = new List<Panel>()
            };

            var createdTeam = _teamService.CreateTeam(team, _adminUser.Id);

            // Act
            _teamService.DeleteTeam(createdTeam, _adminUser.Id);
            
            // Assert
            Assert.AreEqual(0, _teamRepository.GetAllTeams().Count);
        }

        [TestMethod]
        public void DeleteTeam_NonAdminUser_ThrowsException()
        {
            // Arrange
            var nonAdminUser = new User { Id = 2, Name = "New User", LastName = "User LastName",IsAdmin = false , Email = "user2@gmail.com"};
            _userService.AddUser(_adminUser);
            _userService.AddUser(nonAdminUser);

            var team = new Team
            {
                Id = 1,
                Name = "Team D",
                TeamLeader = _adminUser,
                TasksDescription = "Task description for Team D",
                MaxAmountOfMembers = 5,
                Panels = new List<Panel>()
            };

            var createdTeam = _teamService.CreateTeam(team, _adminUser.Id);

            // Act && Assert
            Assert.ThrowsException<UserNotValidException>(() => _teamService.DeleteTeam(createdTeam, nonAdminUser.Id));
        }
        
        

        [TestMethod]
        public void AddUserToTeam_ValidUser_AddsUserSuccessfully()
        {
            // Arrange
            var newUser = new User { Id = 2, Name = "New User", LastName = "User LastName",IsAdmin = false , Email = "user2@gmail.com"};
            _userService.AddUser(_adminUser);
            _userService.AddUser(newUser);

            var team = new Team
            {
                Id = 1,
                Name = "Team E",
                TeamLeader = _adminUser,
                MaxAmountOfMembers = 5,
                TasksDescription = "Task description for Team E",
            };

            var createdTeam = _teamService.CreateTeam(team, _adminUser.Id);

            // Act 
            _teamService.AddUserToTeam(newUser.Id, createdTeam);
            
            // Assert
            Assert.AreEqual(2, createdTeam.Users.Count);
            Assert.IsTrue(createdTeam.Users.Contains(newUser));
        }

        [TestMethod]
        public void AddUserToTeam_TeamIsFull_ThrowsException()
        {
            // Arrange
            var newUser = new User { Id = 2, Name = "New User", LastName = "User LastName",IsAdmin = false , Email = "user2@gmail.com"};
            _userService.AddUser(_adminUser);
            _userService.AddUser(newUser);
            
            var team = new Team
            {
                Id = 1,
                Name = "Team F",
                TeamLeader = _adminUser,
                MaxAmountOfMembers = 1,
                TasksDescription = "Task description for Team F"
            };

            var createdTeam = _teamService.CreateTeam(team, _adminUser.Id);

            // Act & Assert
            Assert.ThrowsException<TeamNotValidException>(() => _teamService.AddUserToTeam(newUser.Id, createdTeam));
        }

        [TestMethod]
        public void UpdateTeam()
        {
            // Arrange
            _userService.AddUser(_adminUser);

            var team = new Team
            {
                Id = 1,
                Name = "Team G",
                TeamLeader = _adminUser,
                MaxAmountOfMembers = 5,
                TasksDescription = "Task description for Team G"
            };

            var createdTeam = _teamService.CreateTeam(team, _adminUser.Id);

            var updatedTeam = new Team
            {
                Id = 1,
                Name = "Team G Updated",
                TeamLeader = _adminUser,
                MaxAmountOfMembers = 5,
                TasksDescription = "Task description for Team G Updated"
            };

            // Act
            _teamService.UpdateTeam(updatedTeam, _adminUser.Id);
            
            // Assert
            Assert.AreEqual(updatedTeam.Name, createdTeam.Name);
            Assert.AreEqual(updatedTeam.TasksDescription, createdTeam.TasksDescription);
        }

        [TestMethod]
        public void GetAllTeams()
        {
            // Arrange
            _userService.AddUser(_adminUser);

            var team1 = new Team
            {
                Id = 1,
                Name = "Team H",
                TeamLeader = _adminUser,
                MaxAmountOfMembers = 5,
                TasksDescription = "Task description for Team H"
            };

            var team2 = new Team
            {
                Id = 2,
                Name = "Team I",
                TeamLeader = _adminUser,
                MaxAmountOfMembers = 5,
                TasksDescription = "Task description for Team I"
            };

            var team3 = new Team
            {
                Id = 3,
                Name = "Team J",
                TeamLeader = _adminUser,
                MaxAmountOfMembers = 5,
                TasksDescription = "Task description for Team J"
            };

            _teamService.CreateTeam(team1, _adminUser.Id);
            _teamService.CreateTeam(team2, _adminUser.Id);
            _teamService.CreateTeam(team3, _adminUser.Id);

            // Act
            var teams = _teamService.GetAllTeams();
            
            // Assert
            Assert.AreEqual(3, teams.Count);
        }

        [TestMethod]
        public void GetTeamById()
        {
            // Arrange
            _userService.AddUser(_adminUser);

            var team = new Team
            {
                Id = 1,
                Name = "Team K",
                TeamLeader = _adminUser,
                MaxAmountOfMembers = 5,
                TasksDescription = "Task description for Team K"
            };

            var createdTeam = _teamService.CreateTeam(team, _adminUser.Id);

            // Act
            var teamById = _teamService.GetTeamById(createdTeam.Id);
            
            // Assert
            Assert.AreEqual(createdTeam.Id, teamById.Id);
        }
        
        [TestMethod]
        public void GetTeamById_TeamDoesNotExist_ReturnsNull()
        {
            // Arrange
            _userService.AddUser(_adminUser);

            var team = new Team
            {
                Id = 1,
                Name = "Team L",
                TeamLeader = _adminUser,
                MaxAmountOfMembers = 5,
                TasksDescription = "Task description for Team L"
            };

            _teamService.CreateTeam(team, _adminUser.Id);

            // Act && assert
            
            Assert.ThrowsException<TeamNotValidException>(() => _teamService.GetTeamById(2));
        }

        [TestMethod]
        public void GetTeamsForUser()
        {
            // Arrange
            _userService.AddUser(_adminUser);

            var team1 = new Team
            {
                Id = 1,
                Name = "Team M",
                TeamLeader = _adminUser,
                MaxAmountOfMembers = 5,
                TasksDescription = "Task description for Team M"
            };

            var team2 = new Team
            {
                Id = 2,
                Name = "Team N",
                TeamLeader = _adminUser,
                MaxAmountOfMembers = 5,
                TasksDescription = "Task description for Team N"
            };

            var team3 = new Team
            {
                Id = 3,
                Name = "Team O",
                TeamLeader = _adminUser,
                MaxAmountOfMembers = 5,
                TasksDescription = "Task description for Team O"
            };

            _teamService.CreateTeam(team1, _adminUser.Id);
            _teamService.CreateTeam(team2, _adminUser.Id);
            _teamService.CreateTeam(team3, _adminUser.Id);

            // Act
            var teams = _teamService.TeamsForUser(_adminUser.Id);
            
            // Assert
            Assert.AreEqual(3, teams.Count);
        }
    }
}
