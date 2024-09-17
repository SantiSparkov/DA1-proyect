using TaskPanel.Models.Entity;
using Task = TaskPanel.Models.Entity.Task;

namespace TaskPanelTest.EntityTest.TashEntityTest;

[TestClass]
public class TrashEntityTest
{
    private Trash trash;
    
    [TestInitialize]
    public void Initialize()
    {
        //Arrange
        trash = new Trash()
        {
            TaskList = new List<Task>(),
            PanelList = new List<Panel>()
        };
        
    }
    
    [TestMethod]
    public void CreateTrashTest()
    {
        //Arrange 
        
        //Act 
        
        // Assert
        Assert.AreEqual(trash.TaskList.Count, 0);
        Assert.AreEqual(trash.PanelList.Count, 0);
    }
    
    [TestMethod]
    public void AddTaskInTrashTest()
    {
        //Arrange 
        Task task = new Task();
        
        //Act 
        trash.TaskList.Add(task);
        
        // Assert
        Assert.AreEqual(trash.TaskList.Count, 1);
        Assert.AreEqual(trash.PanelList.Count, 0);
    }
    
    [TestMethod]
    public void AddPanelInTrashTest()
    {
        //Arrange 
        Panel panel = new Panel();
        
        //Act 
        trash.PanelList.Add(panel);
        
        // Assert
        Assert.AreEqual(trash.TaskList.Count, 0);
        Assert.AreEqual(trash.PanelList.Count, 1);
    }
    
    

    [TestCleanup]
    public void Cleanup()
    {
        trash.PanelList.Clear();
        trash.TaskList.Clear();
    }
}