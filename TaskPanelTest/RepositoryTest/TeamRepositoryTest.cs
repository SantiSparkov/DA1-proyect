using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.Team;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelTest.RepositoryTest;

[TestClass]
public class TeamRepositoryTest
{
    private ITeamRepository _teamRepository;

    [TestInitialize]
    public void Initialize()
    {
        _teamRepository = new TeamRepository();
    }
    
    [TestMethod]
    public void CreateTeamRepository()
    {
        Assert.IsNotNull(_teamRepository);
    }

    [TestMethod]
    public void TeamRepository_AddTeam()
    {
        // Arrange
        var team = new Team
        {
            Id = 1,
            Name = "Team 1",
            CreationDate = DateTime.Now,
            TasksDescription = "Description 1",
            MaxAmountOfMembers = 5,
            Users = new List<User>()
        };
        
        // Act
        _teamRepository.AddTeam(team);
        
        // Assert
        var actualTeam = _teamRepository.GetTeamById(team.Id);
        Assert.IsNotNull(actualTeam, "the team is not added to the repository");
        Assert.AreEqual("Team 1", actualTeam.Name, "the team name is not stored correctly");
    }

    [TestMethod]
    public void TeamRepository_DeleteTeam()
    {
        // Arrange
        var team = new Team
        {
            Id = 1,
            Name = "Team 1",
            CreationDate = DateTime.Now,
            TasksDescription = "Description 1",
            MaxAmountOfMembers = 5,
            Users = new List<User>()
        };
        _teamRepository.AddTeam(team);
        
        // Act
        _teamRepository.DeleteTeam(team.Id);
        
        // Assert
        Assert.ThrowsException<TeamNotValidException>(() => _teamRepository.GetTeamById(team.Id));
    }
    
    [TestMethod]
    public void TeamRepository_GetAllTeams()
    {
        // Arrange
        var team1 = new Team
        {
            Id = 1,
            Name = "Team 1",
            CreationDate = DateTime.Now,
            TasksDescription = "Description 1",
            MaxAmountOfMembers = 5,
            Users = new List<User>()
        };
        var team2 = new Team
        {
            Id = 2,
            Name = "Team 2",
            CreationDate = DateTime.Now,
            TasksDescription = "Description 2",
            MaxAmountOfMembers = 5,
            Users = new List<User>()
        };
        _teamRepository.AddTeam(team1);
        _teamRepository.AddTeam(team2);
        
        // Act
        var actualTeams = _teamRepository.GetAllTeams();
        
        // Assert
        Assert.AreEqual(2, actualTeams.Count, "the amount of teams is not correct");
    }
    
    [TestMethod]
    public void TeamRepository_GetTeamById()
    {
        // Arrange
        var team = new Team
        {
            Id = 1,
            Name = "Team 1",
            CreationDate = DateTime.Now,
            TasksDescription = "Description 1",
            MaxAmountOfMembers = 5,
            Users = new List<User>()
        };
        _teamRepository.AddTeam(team);
        
        // Act
        var actualTeam = _teamRepository.GetTeamById(team.Id);
        
        // Assert
        Assert.IsNotNull(actualTeam, "the team is not found in the repository");
        Assert.AreEqual("Team 1", actualTeam.Name, "the team name is not correct");
    }
    
    [TestMethod]
    public void TeamRepository_UpdateTeam()
    {
        // Arrange
        var team = new Team
        {
            Id = 1,
            Name = "Team 1",
            CreationDate = DateTime.Now,
            TasksDescription = "Description 1",
            MaxAmountOfMembers = 5,
            Users = new List<User>()
        };
        _teamRepository.AddTeam(team);
        
        // Act
        var updatedTeam = new Team
        {
            Id = team.Id,
            Name = "Team 2",
            MaxAmountOfMembers = 10
        };
        
        _teamRepository.UpdateTeam(updatedTeam);
        
        // Assert
        var actualTeam = _teamRepository.GetTeamById(team.Id);
        Assert.AreEqual("Team 2", actualTeam.Name, "the team name is not updated");
        Assert.AreEqual(10, actualTeam.MaxAmountOfMembers, "the team max amount of members is not updated");
        Assert.IsNotNull(actualTeam, "the team is not found in the repository");
    }
    
    [TestMethod]
    public void UpdateTeam_WithNullTeam()
    {
        // Arrange
        var team = new Team
        {
            Id = 1,
            Name = "Team 1",
            CreationDate = DateTime.Now,
            TasksDescription = "Description 1",
            MaxAmountOfMembers = 5,
            Users = new List<User>()
        };
        
        // Act & Assert
        var exception = Assert.ThrowsException<TeamNotValidException>(new Action(() => _teamRepository.UpdateTeam(team)));
        Assert.AreEqual($"Team with id 1 not found", exception.Message);
    }
    
    [TestMethod]
    public void DeleteTeam_WithNonExistentTeam()
    {
        // Arrange
        var team = new Team
        {
            Id = 1,
            Name = "Team 1",
            CreationDate = DateTime.Now,
            TasksDescription = "Description 1",
            MaxAmountOfMembers = 5,
            Users = new List<User>()
        };
        _teamRepository.AddTeam(team);
        
        // Act & Assert
        var exception = Assert.ThrowsException<TeamNotValidException>(new Action(() => _teamRepository.DeleteTeam(2)));
        Assert.AreEqual("Team with id 2 not found", exception.Message);
    }
    
    [TestCleanup]
    public void Cleanup()
    {
        var teams = _teamRepository.GetAllTeams();
        var teamsToDelete = teams.ToList();
        foreach (var team in teamsToDelete)
        {
            _teamRepository.DeleteTeam(team.Id);
        }
    }
}

