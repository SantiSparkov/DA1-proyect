namespace TaskPanelTest.EntityTest.TaskEntityTest;

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
            Priority = Priority.Hight,
            CommentList = new List<Comment>(),
        };
        
        //Act 
        
        // Assert
        Assert.AreEqual("Title", task.Title);
        Assert.AreEqual("Description", task.Description);
        Assert.AreEqual(new DateTime(2008, 6, 1, 7, 47, 0), task.DueDate);
        Assert.AreEqual(Priority.Urgente, task.Priority);
        Assert.IsNotNull(task.CommentList);

    }
}