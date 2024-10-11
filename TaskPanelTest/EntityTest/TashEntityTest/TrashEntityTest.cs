using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception;
using Task = TaskPanelLibrary.Entity.Task;

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
        trash.AddPanel(panel1);
        trash.AddPanel(panel2);
        trash.AddPanel(panel3);
        trash.AddPanel(panel4);
        trash.AddPanel(panel5);
        trash.AddPanel(panel6);
        trash.AddPanel(panel7);
        trash.AddPanel(panel8);
        trash.AddPanel(panel9);
        trash.AddPanel(panel10);
        
        
        // Assert
        var ex = Assert.ThrowsException<ApiException>(() => trash.AddPanel(panel11));
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
        trash.AddTask(task1);
        trash.AddTask(task2);
        trash.AddTask(task3);
        trash.AddTask(task4);
        trash.AddTask(task5);
        trash.AddTask(task6);
        trash.AddTask(task7);
        trash.AddTask(task8);
        trash.AddTask(task9);
        trash.AddTask(task10);
        
        
        
        // Assert
        var ex = Assert.ThrowsException<ApiException>(() => trash.AddTask(task11));
        Assert.AreEqual("Papelera llena", ex.Message);
    }

    [TestCleanup]
    public void Cleanup()
    {
        trash.PanelList.Clear();
        trash.TaskList.Clear();
    }
}