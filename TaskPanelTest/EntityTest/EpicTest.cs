using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Entity.Enum;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.EntityTest;

[TestClass]
public class EpicTest
{
    [TestMethod]
    public void TestEpicEntity()
    {
        //Arrange
        Epic epic = new Epic()
        {
            Id = 1,
            Title = "Test",
            Priority = EPriority.LOW,
            Description = "Desc test",
            DueDateTime = new DateTime(2024, 12, 12),
            PanelId = 1,
            Tasks = new List<Task>()
        };
        
        // ASSERT
        Assert.AreEqual(1,epic.Id);
        Assert.AreEqual(1,epic.PanelId);
        Assert.AreEqual(EPriority.LOW, epic.Priority);
        Assert.AreEqual("Test", epic.Title);
        Assert.AreEqual(new DateTime(2024, 12, 12), epic.DueDateTime);
        Assert.IsNotNull(epic.Tasks);
    }
    
}