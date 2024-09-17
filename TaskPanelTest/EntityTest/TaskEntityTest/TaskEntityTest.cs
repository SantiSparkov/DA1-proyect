namespace TaskPanelTest.EntityTest.TaskEntityTest;
using TaskPanel.Models.Entity;
using TaskPriority = TaskPanel.Models.Entity.Task.TaskPriority;

[TestClass]
public class TaskEntityTest
{
    [TestMethod]
    public void CreateTaskTest()
    {
        //Arrange 
        Task task = new Task()
        {
            Title = "Title",
            Description = "Description",
            DueDate = new DateTime(2008, 6, 1, 7, 47, 0),
            Priority = TaskPriority.High,
            CommentList = new List<Comment>(),
        };
        
        //Act 
        
        // Assert
        Assert.AreEqual("Title", task.Title);
        Assert.AreEqual("Description", task.Description);
        Assert.AreEqual(new DateTime(2008, 6, 1, 7, 47, 0), task.DueDate);
        Assert.AreEqual(TaskPriority.High, task.Priority);
        Assert.IsNotNull(task.CommentList);

    }
}