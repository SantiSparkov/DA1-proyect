namespace TaskPanelTest.EntityTest.PanelEntityTest;
using TaskPanel.Models.Entity;
using TaskPriority = TaskPanel.Models.Entity.Task.TaskPriority;

[TestClass]
public class PanelEntityTest
{
    [TestMethod]
    public void CreatePanelTest()
    {
        //Arrange 
        Panel panel = new Panel()
        {
            Id = 1,
            Name = "Panel1",
            Team = new Team(),
            Description = "description",
            Tasks = new List<Task>()
        };
        
        // Assert
        Assert.AreEqual(1, panel.Id);
        Assert.AreEqual("Panel1", panel.Name);
        Assert.IsNotNull(panel.Team);
        Assert.AreEqual("description", panel.Description);
        Assert.IsNotNull(panel.Tasks);

    }
}