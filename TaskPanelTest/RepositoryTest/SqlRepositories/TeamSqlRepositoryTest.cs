using TaskPanelLibrary.Config;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelTest.ConfigTest;

namespace TaskPanelTest.RepositoryTest.SqlRepositories;

[TestClass]
public class TeamSqlRepositoryTest
{
    
    private SqlContext _sqlContext;

    private ITeamRepository _teamRepository;
    
    [TestInitialize]
    public void Initialize()
    {
        _sqlContext = new SqlContexTest().CreateMemoryContext();
        _teamRepository = new TeamSqlRepository(_sqlContext);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext?.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void AddPanel()
    {
        //Arrange
        Team? team = new Team()
        {
            Id = 1,
            CreationDate = new DateTime(2024,5, 22),
            MaxAmountOfMembers = 10,
            Name = "Team test",
            Panels = new List<Panel>(),
            TasksDescription = "Team description",
            TeamLeaderId = 1,
            Users = new List<User>()
        };
        
        //Act
        _teamRepository.AddTeam(team);
        List<Team?> teams = _teamRepository.GetAllTeams();
        
        //Assert
        Assert.IsNotNull(teams);
        Assert.AreEqual(1, teams.Count());
    }
    
    [TestMethod]
    public void GetTeamById()
    {
        //Arrange
        Team? team = new Team()
        {
            Id = 1,
            CreationDate = new DateTime(2024,5, 22),
            MaxAmountOfMembers = 10,
            Name = "Team test",
            Panels = new List<Panel>(),
            TasksDescription = "Team description",
            TeamLeaderId = 1,
            Users = new List<User>()
        };
        
        //Act
        _teamRepository.AddTeam(team);
        Team? teamSaved = _teamRepository.GetTeamById(team.Id);
        
        //Assert
        Assert.AreEqual(team.Id, teamSaved.Id);
        Assert.AreEqual(team.CreationDate, teamSaved.CreationDate);
        Assert.AreEqual(team.MaxAmountOfMembers, teamSaved.MaxAmountOfMembers);
        Assert.AreEqual(team.Name, teamSaved.Name);
        Assert.AreEqual(team.TasksDescription, teamSaved.TasksDescription);
        Assert.AreEqual(team.TeamLeaderId, teamSaved.TeamLeaderId);
        Assert.IsNotNull(teamSaved.Panels);
        Assert.IsNotNull(teamSaved.Users);        
    }
    
    [TestMethod]
    public void GetTeamByIdNotExist()
    {
        //Arrange
        Team team = new Team()
        {
            Id = 1,
            CreationDate = new DateTime(2024,5, 22),
            MaxAmountOfMembers = 10,
            Name = "Team test",
            Panels = new List<Panel>(),
            TasksDescription = "Team description",
            TeamLeaderId = 1,
            Users = new List<User>()
        };
        
        //Act 
        var exception = Assert.ThrowsException<System.Exception>(() => _teamRepository.GetTeamById(team.Id));

        // Assert
        Assert.AreEqual($"Team with id: {team.Id} does not exist", exception.Message);
    }
    
    [TestMethod]
    public void UpdateTeam()
    {
        //Arrange
        Team? team = new Team()
        {
            Id = 1,
            CreationDate = new DateTime(2024,5, 22),
            MaxAmountOfMembers = 10,
            Name = "Team test",
            Panels = new List<Panel>(),
            TasksDescription = "Team description",
            TeamLeaderId = 1,
            Users = new List<User>()
        };
        
        //Act
        _teamRepository.AddTeam(team);
        team.Name = "Update name team";
        team.TasksDescription = "Description updated";
        
        Team? teamUpdated = _teamRepository.UpdateTeam(team);
        
        //Assert
        Assert.AreEqual("Update name team", teamUpdated.Name);
        Assert.AreEqual("Description updated", teamUpdated.TasksDescription);      
    }
    
    [TestMethod]
    public void DeleteTeam()
    {
        //Arrange
        Team team = new Team()
        {
            Id = 1,
            CreationDate = new DateTime(2024,5, 22),
            MaxAmountOfMembers = 10,
            Name = "Team test",
            Panels = new List<Panel>(),
            TasksDescription = "Team description",
            TeamLeaderId = 1,
            Users = new List<User>()
        };
        
        //Act
        _teamRepository.AddTeam(team);
        Team teamDelete = _teamRepository.DeleteTeam(team.Id);
        
        //Assert
        Assert.AreEqual(0, _teamRepository.GetAllTeams().Count);      
    }
    
    [TestMethod]
    public void DeleteTeamNotExist()
    {
        //Arrange
        
        //Act 
        var exception = Assert.ThrowsException<System.Exception>(() => _teamRepository.DeleteTeam(1));

        // Assert
        Assert.AreEqual($"Team with id: {1} does not exist", exception.Message);
    }
    
}