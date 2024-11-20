using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using Task = TaskPanelLibrary.Entity.Task;

namespace TaskPanelTest.EntityTest;

[TestClass]
public class TrashEntityTest
{
    private Trash trash;
    
    [TestInitialize]
    public void Initialize()
    {
        trash = new Trash();
    }
    
    [TestMethod]
    public void TestTrashEntity()
    {
        Trash trash = new Trash()
        {
            Id = 1,
            Elements = 10,
            PanelList = new List<Panel>(),
            TaskList = new List<Task>(),
            UserId = 1
        };
        
        // ASSERT

        Assert.AreEqual(1, trash.Id);
        Assert.IsNotNull(trash.PanelList);
        Assert.IsNotNull(trash.TaskList);
        Assert.AreEqual(10, trash.Elements);
        Assert.AreEqual(1, trash.UserId);
    }
}