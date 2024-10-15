using TaskPanelLibrary.Entity;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.EntityTest;

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