using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Exception.Team;
using TaskPanelLibrary.Exception.User;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelTest.ServiceTest
{
    [TestClass]
    public class TeamServiceTests
    {
        private Mock<ITeamRepository> _mockTeamRepository;
        private Mock<IUserService> _mockUserService;
        private TeamService _teamService;

        [TestInitialize]
        public void Setup()
        {
            _mockTeamRepository = new Mock<ITeamRepository>();
            _mockUserService = new Mock<IUserService>();

            _teamService = new TeamService(
                _mockTeamRepository.Object,
                _mockUserService.Object
            );
        }

        [TestMethod]
        public void CreateTeam_ShouldCreateTeam_WhenUserIsAdmin()
        {
            // Arrange
            var user = new User { Id = 1, IsAdmin = true };
            var team = new Team
            {
                Name = "Valid Team",
                TasksDescription = "Valid Description",
                MaxAmountOfMembers = 5,
                TeamLeaderId = 1,
                Panels = new List<Panel>(),
                Users = new List<User> { user },
                CreationDate = System.DateTime.Now,
                Id = 1
            };

            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);
            _mockTeamRepository.Setup(repo => repo.GetAllTeams()).Returns(new List<Team>());
            _mockTeamRepository.Setup(repo => repo.AddTeam(It.IsAny<Team>())).Returns(team);

            // Act
            var createdTeam = _teamService.CreateTeam(team, user.Id);

            // Assert
            Assert.AreEqual(team.Name, createdTeam.Name);
            Assert.AreEqual(team.TasksDescription, createdTeam.TasksDescription);
            Assert.AreEqual(team.MaxAmountOfMembers, createdTeam.MaxAmountOfMembers);
            Assert.AreEqual(team.TeamLeaderId, createdTeam.TeamLeaderId);
            Assert.AreEqual(team.Panels, createdTeam.Panels);
            Assert.AreEqual(team.Users, createdTeam.Users);
            Assert.AreEqual(team.CreationDate, createdTeam.CreationDate);
            Assert.AreEqual(team.Id, createdTeam.Id);
        }

        [TestMethod]
        public void CreateTeam_ShouldThrowException_WhenUserIsNotAdmin()
        {
            // Arrange
            var user = new User { Id = 1, IsAdmin = false };
            var team = new Team { Name = "Valid Team", TasksDescription = "Valid Description", MaxAmountOfMembers = 5 };

            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);

            // Act & Assert
            Assert.ThrowsException<UserNotValidException>(() => _teamService.CreateTeam(team, user.Id));
        }

        [TestMethod]
        public void CreateTeam_ShouldThrowException_WhenTeamNameIsNull()
        {
            // Arrange
            var user = new User { Id = 1, IsAdmin = true };
            var team = new Team { Name = null, TasksDescription = "Valid Description", MaxAmountOfMembers = 5 };

            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);

            // Act & Assert
            Assert.ThrowsException<TeamNotValidException>(() => _teamService.CreateTeam(team, user.Id));
        }

        [TestMethod]
        public void CreateTeam_ShouldThrowException_WhenMaxAmountOfMembersIsZero()
        {
            // Arrange
            var user = new User { Id = 1, IsAdmin = true };
            var team = new Team { Name = "Valid Team", TasksDescription = "Valid Description", MaxAmountOfMembers = 0 };

            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);

            // Act & Assert
            Assert.ThrowsException<TeamNotValidException>(() => _teamService.CreateTeam(team, user.Id));
        }

        [TestMethod]
        public void CreateTeam_ShouldThrowException_WhenTeamNameIsNotUnique()
        {
            // Arrange
            var user = new User { Id = 1, IsAdmin = true };
            var team = new Team
                { Name = "Duplicate Team", TasksDescription = "Valid Description", MaxAmountOfMembers = 5 };

            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);
            _mockTeamRepository.Setup(repo => repo.GetAllTeams())
                .Returns(new List<Team> { new Team { Name = "Duplicate Team" } });

            // Act & Assert
            Assert.ThrowsException<TeamNotValidException>(() => _teamService.CreateTeam(team, user.Id));
        }

        [TestMethod]
        public void DeleteTeam_ShouldDeleteTeam_WhenUserIsAdmin()
        {
            // Arrange
            var user = new User { Id = 1, IsAdmin = true };
            var team = new Team { Id = 1, Panels = new List<Panel>() };

            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);
            _mockTeamRepository.Setup(repo => repo.DeleteTeam(It.IsAny<int>())).Returns(team);

            // Act
            var deletedTeam = _teamService.DeleteTeam(team, user.Id);

            // Assert
            Assert.AreEqual(team.Id, deletedTeam.Id);
        }

        [TestMethod]
        public void DeleteTeam_ShouldThrowException_WhenUserIsNotAdmin()
        {
            // Arrange
            var user = new User { Id = 1, IsAdmin = false };
            var team = new Team { Id = 1, Panels = new List<Panel>() };

            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);

            // Act & Assert
            Assert.ThrowsException<UserNotValidException>(() => _teamService.DeleteTeam(team, user.Id));
        }

        [TestMethod]
        public void DeleteTeam_ShouldThrowException_WhenTeamHasPanels()
        {
            // Arrange
            var user = new User { Id = 1, IsAdmin = true };
            var team = new Team { Id = 1, Panels = new List<Panel> { new Panel() } };

            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);

            // Act & Assert
            Assert.ThrowsException<TeamNotValidException>(() => _teamService.DeleteTeam(team, user.Id));
        }

        [TestMethod]
        public void UpdateTeam_ShouldUpdateTeam_WhenUserIsAdminAndTeamIsValid()
        {
            // Arrange
            var user = new User { Id = 1, IsAdmin = true };
            var team = new Team
            {
                Id = 1, Name = "Updated Team", MaxAmountOfMembers = 5, TeamLeaderId = 1,
                TasksDescription = "Valid Description"
            };

            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);
            _mockTeamRepository.Setup(repo => repo.GetTeamById(It.IsAny<int>())).Returns(team);
            _mockTeamRepository.Setup(repo => repo.UpdateTeam(It.IsAny<Team>())).Returns(team);

            // Act
            var updatedTeam = _teamService.UpdateTeam(team, user.Id);

            // Assert
            Assert.AreEqual(team.Name, updatedTeam.Name);
        }

        [TestMethod]
        public void UpdateTeam_ShouldThrowException_WhenUserIsNotAdmin()
        {
            // Arrange
            var user = new User { Id = 1, IsAdmin = false };
            var team = new Team { Id = 1, Name = "Updated Team", MaxAmountOfMembers = 5 };

            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);

            // Act
            var exception = Assert.ThrowsException<UserNotValidException>(() => _teamService.UpdateTeam(team, user.Id));

            // Assert
            Assert.AreEqual("User is not admin or team leader.", exception.Message);
        }

        [TestMethod]
        public void GetTeamById_ShouldReturnTeam_WhenTeamExists()
        {
            // Arrange
            var team = new Team { Id = 1 };
            _mockTeamRepository.Setup(repo => repo.GetTeamById(It.IsAny<int>())).Returns(team);

            // Act
            var result = _teamService.GetTeamById(1);
            
            // Assert
            Assert.AreEqual(team.Id, result.Id);
        }

        [TestMethod]
        public void GetAllTeams_ShouldReturnAllTeams()
        {
            // Arrange
            var teams = new List<Team>
            {
                new Team { Id = 1 },
                new Team { Id = 2 }
            };
            _mockTeamRepository.Setup(repo => repo.GetAllTeams()).Returns(teams);

            // Act
            var result = _teamService.GetAllTeams();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void TeamsForUser_ShouldReturnTeams_WhenUserBelongsToTeams()
        {
            // Arrange
            var userId = 1;
            var teams = new List<Team>
            {
                new Team { Id = 1, Users = new List<User> { new User { Id = userId } } },
                new Team { Id = 2, Users = new List<User> { new User { Id = 2 } } }
            };

            _mockTeamRepository.Setup(repo => repo.GetAllTeams()).Returns(teams);

            // Act
            var result = _teamService.TeamsForUser(userId);

            // Assert
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual(1, result[0].Id);
        }

        [TestMethod]
        public void IsValidTeam_ShouldThrowException_WhenTasksDescriptionIsEmpty()
        {
            // Arrange
            var user = new User { Id = 1, IsAdmin = true };
            var team = new Team { Id = 1, Name = "Team A", MaxAmountOfMembers = 5 };

            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);
            _mockTeamRepository.Setup(repo => repo.GetAllTeams()).Returns(new List<Team>());

            // Act & Assert
            Assert.ThrowsException<TaskNotValidException>(() => _teamService.CreateTeam(team, user.Id));
        }

        [TestMethod]
        public void CanUpdateTeam_ShouldThrowException_WhenMaxAmountOfMembersIsLessThanCurrentUsers()
        {
            // Arrange
            var user = new User { Id = 1, IsAdmin = true };
            var updatedTeam = new Team { Id = 1, MaxAmountOfMembers = 2, TeamLeaderId = 1 };
            var existingTeam = new Team
            {
                Id = 1, TasksDescription = "A", TeamLeaderId = 1,
                Users = new List<User> { new User(), new User(), new User() }
            };

            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);
            _mockTeamRepository.Setup(repo => repo.GetTeamById(It.IsAny<int>())).Returns(existingTeam);

            // Act 
            var exception =
                Assert.ThrowsException<TeamNotValidException>(() => _teamService.UpdateTeam(updatedTeam, user.Id));

            // Assert
            Assert.AreEqual("Cannot set the maximum number of users lower than the current number of users.",
                exception.Message);
        }

        [TestMethod]
        public void CanUpdateTeam_ShouldThrowException_WhenTasksDescriptionIsNull()
        {
            // Arrange
            var user = new User { Id = 1, IsAdmin = true };
            var updatedTeam = new Team { Id = 1, TeamLeaderId = 1 };
            var existingTeam = new Team { Id = 1, TeamLeaderId = 1, TasksDescription = "Existing Description" };

            _mockUserService.Setup(service => service.GetUserById(It.IsAny<int>())).Returns(user);
            _mockTeamRepository.Setup(repo => repo.GetTeamById(It.IsAny<int>())).Returns(existingTeam);
            _mockTeamRepository.Setup(repo => repo.UpdateTeam(It.IsAny<Team>())).Returns(existingTeam);

            // Act & Assert
            Assert.ThrowsException<TeamNotValidException>(() => _teamService.UpdateTeam(updatedTeam, user.Id));
        }

        [TestMethod]
        public void TeamsForUser_ShouldReturnEmptyList_WhenUserDoesNotBelongToAnyTeam()
        {
            // Arrange
            var userId = 1;
            var teams = new List<Team>
            {
                new Team { Id = 1, Users = new List<User> { new User { Id = 2 } } },
                new Team { Id = 2, Users = new List<User> { new User { Id = 3 } } }
            };

            _mockTeamRepository.Setup(repo => repo.GetAllTeams()).Returns(teams);

            // Act
            var result = _teamService.TeamsForUser(userId);

            // Assert
            Assert.AreEqual(0, result.Count);
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