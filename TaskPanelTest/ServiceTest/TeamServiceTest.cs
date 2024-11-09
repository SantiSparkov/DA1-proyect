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
        
        private UserSqlRepository _userSqlRepository;
        
        private IPanelRepository _panelRepository;
        
        private IPanelService _panelService;
        
        private IUserService _userService;
        
        private PasswordGeneratorService _passwordGeneratorService;
        
        private User _adminUser;

        [TestInitialize]
        public void Initialize()
        {
            _teamRepository = new TeamRepository();
            _userSqlRepository = new UserSqlRepository(null);
            _panelRepository = new PanelRepository();
            
            _passwordGeneratorService = new PasswordGeneratorService();
            _userService = new UserService(_userSqlRepository, _passwordGeneratorService);
            _panelService = new PanelService(_panelRepository, _userService);
            _teamService = new TeamService(_teamRepository, _userService, _panelService);
            
            _adminUser = new User {Name = "Admin User", IsAdmin = true, Email = "userAdministrator@gmail.com", 
                LastName = "LasatMan", BirthDate = new System.DateTime(1990, 1, 1)};
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
            Assert.AreEqual(_adminUser.Id, createdTeam.TeamLeaderId);
            Assert.AreEqual(1, createdTeam.Users.Count);
        }

        [TestMethod]
        public void CreateTeam_NonAdminUser_ThrowsException()
        {
            // Arrange
            var nonAdminUser = new User { Id = 2, Name = "New User", LastName = "User LastName",IsAdmin = false , 
                Email = "user2@gmail.com", BirthDate = new System.DateTime(1991, 1, 1)};
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
                TeamLeaderId = _adminUser.Id,
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
            var nonAdminUser = new User { Id = 2, Name = "New User", LastName = "User LastName",IsAdmin = false , 
                Email = "user2@gmail.com", BirthDate = new System.DateTime(1991, 1, 1)};
            
            _userService.AddUser(_adminUser);
            _userService.AddUser(nonAdminUser);

            var team = new Team
            {
                Id = 1,
                Name = "Team D",
                TeamLeaderId = _adminUser.Id,
                TasksDescription = "Task description for Team D",
                MaxAmountOfMembers = 5,
                Panels = new List<Panel>()
            };

            var createdTeam = _teamService.CreateTeam(team, _adminUser.Id);

            // Act && Assert
            Assert.ThrowsException<UserNotValidException>(() => _teamService.DeleteTeam(createdTeam, nonAdminUser.Id));
        }
        
    }
}
