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
            
            _adminUser = new User {Name = "Admin User", IsAdmin = true, Email = "userAdministrator@gmail.com"};
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
            var nonAdminUser = new User { Id = 2, Name = "Non-admin User", IsAdmin = false };
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
        public void DeleteTeam_ValidTeam_DeletesTeamSuccessfully()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Admin User", IsAdmin = true };
            _userService.AddUser(user);

            var team = new Team
            {
                Id = 1,
                Name = "Team C",
                TeamLeader = user,
                MaxAmountOfMembers = 5,
                TasksDescription = "Task description for Team C",
                Panels = new List<Panel>()
            };

            var createdTeam = _teamService.CreateTeam(team, user.Id);

            // Act
            _teamService.DeleteTeam(createdTeam, user.Id);
            
            // Assert
            Assert.AreEqual(0, _teamRepository.GetAllTeams().Count);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotValidException))]
        public void DeleteTeam_NonAdminUser_ThrowsException()
        {
            // Arrange
            var adminUser = new User { Id = 1, Name = "Admin User", IsAdmin = true };
            var nonAdminUser = new User { Id = 2, Name = "Non-admin User", IsAdmin = false };
            _userService.AddUser(adminUser);
            _userService.AddUser(nonAdminUser);

            var team = new Team
            {
                Id = 1,
                Name = "Team D",
                TeamLeader = adminUser,
                MaxAmountOfMembers = 5,
                Panels = new List<Panel>()
            };

            _teamRepository.AddTeam(team);

            // Act
            _teamService.DeleteTeam(team, nonAdminUser.Id);
        }

        [TestMethod]
        public void AddUserToTeam_ValidUser_AddsUserSuccessfully()
        {
            // Arrange
            var newUser = new User { Id = 2, Name = "New User", IsAdmin = false };
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
            var newUser = new User { Id = 2, Name = "New User", IsAdmin = false };
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
    }
}
