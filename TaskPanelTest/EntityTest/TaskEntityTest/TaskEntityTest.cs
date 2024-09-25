using TaskPanelLibrary.Entity;
using Task = TaskPanelLibrary.Entity.Task;

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
            TaskId = 1,
            Title = "Title",
            Description = "Description",
            DueDate = new DateTime(2008, 6, 1, 7, 47, 0),
            Priority = Task.TaskPriority.HIGH,
            CommentList = new List<Comment>(),
        };
        
        // Assert
        Assert.AreEqual(1, task.TaskId);
        Assert.AreEqual("Title", task.Title);
        Assert.AreEqual("Description", task.Description);
        Assert.AreEqual(new DateTime(2008, 6, 1, 7, 47, 0), task.DueDate);
        Assert.AreEqual(Task.TaskPriority.HIGH, task.Priority);
        Assert.IsNotNull(task.CommentList);

    }
    
    [TestMethod]
    public void AddCommentTest()
    {
        //Arrange 
        Task task = new Task()
        {
            TaskId = 1,
            Title = "Title",
            Description = "Description",
            DueDate = new DateTime(2008, 6, 1, 7, 47, 0),
            Priority = Task.TaskPriority.HIGH,
            CommentList = new List<Comment>(),
        };

        Comment comment = new Comment();
        
        //Act 
        task.AddComment(comment);
        
        // Assert
        Assert.AreEqual(1, task.CommentList.Count);
        Assert.AreEqual(comment, task.CommentList[0]);
    }
}