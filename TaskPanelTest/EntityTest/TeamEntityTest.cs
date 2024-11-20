using TaskPanelLibrary.Entity;

namespace TaskPanelTest.EntityTest;

[TestClass]
public class TeamEntityTest
{
    [TestMethod]
    public void CreateTeamTest()
    {
        //Arrange 
        Team team = new Team()
        {
            Name = "Team Name",
            CreationDate = new DateTime(),
            TeamLeaderId = 1,
            TasksDescription = "task description",
            MaxAmountOfMembers = 1,
            Panels = new List<Panel>(),
            Users = new List<User>()
        };
        
        // Assert
        Assert.AreEqual("Team Name", team.Name);
        Assert.IsNotNull(team.CreationDate);
        Assert.AreEqual("task description", team.TasksDescription);
        Assert.AreEqual(1, team.MaxAmountOfMembers);
        Assert.AreEqual(1, team.TeamLeaderId);
        Assert.IsNotNull(team.Users);
        Assert.IsNotNull(team.Panels);

    }
}