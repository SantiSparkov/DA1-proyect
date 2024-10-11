using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Exception.User;
using TaskPanelLibrary.Exception.Team;
using TaskPanelLibrary.Repository;

namespace TaskPanelTest.ServiceTest
{
    [TestClass]
    public class TeamServiceTest
    {
        private TeamService _teamService;
        private UserService _userService;
        private PanelService _panelService;
        private PanelRepository _panelRepository;
        private TaskService _taskService;
        private TeamRepository _teamRepository;

        [TestInitialize]
        public void Initialize()
        {
            _teamRepository = new TeamRepository();
            _userService = new UserService();
            _panelService = new PanelService(_panelRepository, _taskService);
            _teamService = new TeamService(_teamRepository, _userService, _panelService);
        }

        [TestMethod]
        public void CreateTeam_ValidTeam_CreatesTeamSuccessfully()
        {
            // Arrange
            var user = new User { Id = 1, Name = "Admin User", IsAdmin = true };
            _userService.AddUser(user);

            var team = new Team
            {
                Name = "Team A",
                TasksDescription = "Task description for Team A",
                MaxAmountOfMembers = 5
            };

            // Act
            var createdTeam = _teamService.CreateTeam(team, user.Id);

            // Assert
            Assert.IsNotNull(createdTeam);
            Assert.AreEqual(team.Name, createdTeam.Name);
            Assert.AreEqual(user.Id, createdTeam.TeamLeader.Id);
            Assert.AreEqual(1, createdTeam.Users.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(UserNotValidException))]
        public void CreateTeam_NonAdminUser_ThrowsException()
        {
            // Arrange
            var user = new User { Id = 2, Name = "Non-admin User", IsAdmin = false };
            _userService.AddUser(user);

            var team = new Team
            {
                Name = "Team B",
                TasksDescription = "Task description for Team B",
                MaxAmountOfMembers = 3
            };

            // Act
            _teamService.CreateTeam(team, user.Id);

            // Assert is handled by the ExpectedException
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
                Panels = new List<Panel>()
            };

            _teamRepository.AddTeam(team); // Simulate team already existing in the system

            // Act
            var deletedTeam = _teamService.DeleteTeam(team, user.Id);

            // Assert
            Assert.AreEqual(team.Id, deletedTeam.Id);
            Assert.IsNull(_teamRepository.GetTeamById(team.Id)); // Ensure the team was deleted
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
            var adminUser = new User { Id = 1, Name = "Admin User", IsAdmin = true };
            var newUser = new User { Id = 2, Name = "New User", IsAdmin = false };
            _userService.AddUser(adminUser);
            _userService.AddUser(newUser);

            var team = new Team
            {
                Id = 1,
                Name = "Team E",
                TeamLeader = adminUser,
                MaxAmountOfMembers = 5,
                Users = new List<User> { adminUser }
            };

            _teamRepository.AddTeam(team);

            // Act
            _teamService.AddUserToTeam(newUser.Id, team);

            // Assert
            Assert.AreEqual(2, team.Users.Count);
            Assert.IsTrue(team.Users.Contains(newUser));
        }

        [TestMethod]
        public void AddUserToTeam_TeamIsFull_ThrowsException()
        {
            // Arrange
            var adminUser = new User { Id = 1, Name = "Admin User", IsAdmin = true };
            _userService.AddUser(adminUser);

            var team = new Team
            {
                Id = 1,
                Name = "Team F",
                TeamLeader = adminUser,
                MaxAmountOfMembers = 1,
                Users = new List<User> { adminUser }
            };

            _teamService.CreateTeam(team, adminUser.Id);
            _teamRepository.AddTeam(team);

            var newUser = new User { Id = 2, Name = "New User", IsAdmin = false };
            _userService.AddUser(newUser);

            // Act
            _teamService.AddUserToTeam(newUser.Id, team);
            
            // Assert
            Assert.AreEqual(2, team.Users.Count);
            Assert.IsTrue(team.Users.Contains(newUser));
            
        }
    }
}
