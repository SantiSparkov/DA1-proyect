using TaskPanel.Models.Entity;

namespace TaskPanelTest.EntityTest.TashEntityTest;

[TestClass]
public class TrashEntityTest
{
    [TestMethod]
    public void CreateTrashTest()
    {
        //Arrange 
        Trash trash = new Trash()
        {
            TaskList = new LinkedList<Object>(),
            PanelList = new LinkedList<Object>()
        };
        
        //Act 
        
        // Assert
        Assert.AreEqual(trash.TaskList.Count, 0);
        Assert.AreEqual(trash.PanelList.Count, 0);
    }
}