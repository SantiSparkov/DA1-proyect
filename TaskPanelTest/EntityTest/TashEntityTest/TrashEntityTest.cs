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
        trash = new Trash();
        
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
    
    [TestMethod]
    public void Add11PanelTrashTest()
    {
        //Arrange 
        Panel panel1 = new Panel();
        Panel panel2 = new Panel();
        Panel panel3 = new Panel();
        Panel panel4 = new Panel();
        Panel panel5 = new Panel();
        Panel panel6 = new Panel();
        Panel panel7 = new Panel();
        Panel panel8 = new Panel();
        Panel panel9 = new Panel();
        Panel panel10 = new Panel();
        Panel panel11 = new Panel();
        
        //Act 
        trash.addPanel(panel1);
        trash.addPanel(panel2);
        trash.addPanel(panel3);
        trash.addPanel(panel4);
        trash.addPanel(panel5);
        trash.addPanel(panel6);
        trash.addPanel(panel7);
        trash.addPanel(panel8);
        trash.addPanel(panel9);
        trash.addPanel(panel10);
        
        
        // Assert
        var ex = Assert.ThrowsException<Exception>(() => trash.addPanel(panel11));
        Assert.AreEqual("Papelera llena", ex.Message);
    }
    
    [TestMethod]
    public void Add11TaskTrashTest()
    {
        //Arrange 
        Task task1 = new Task();
        Task task2 = new Task();
        Task task3 = new Task();
        Task task4 = new Task();
        Task task5 = new Task();
        Task task6 = new Task();
        Task task7 = new Task();
        Task task8 = new Task();
        Task task9 = new Task();
        Task task10 = new Task();
        Task task11 = new Task();
        
        
        //Act 
        trash.addTask(task1);
        trash.addTask(task2);
        trash.addTask(task3);
        trash.addTask(task4);
        trash.addTask(task5);
        trash.addTask(task6);
        trash.addTask(task7);
        trash.addTask(task8);
        trash.addTask(task9);
        trash.addTask(task10);
        
        
        
        // Assert
        var ex = Assert.ThrowsException<Exception>(() => trash.addTask(task11));
        Assert.AreEqual("Papelera llena", ex.Message);
    }

    [TestCleanup]
    public void Cleanup()
    {
        trash.PanelList.Clear();
        trash.TaskList.Clear();
    }
}