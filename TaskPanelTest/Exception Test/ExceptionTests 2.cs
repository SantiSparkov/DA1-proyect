using TaskPanelLibrary.Exception;
using TaskPanelLibrary.Exception.Panel;
using TaskPanelLibrary.Exception.Task;
using TaskPanelLibrary.Exception.Team;
using TaskPanelLibrary.Exception.User;

namespace TaskPanelTest.Exception_Test;

[TestClass]
public class ExceptionTests
{
    [TestMethod]
    public void TrashNotValidExceptionTest()
    {
        // ARRANGE
        TrashNotValidException trashNotValidException = new TrashNotValidException("Trash not valid");

        // ASSERT
        Assert.AreEqual("Trash not valid", trashNotValidException.Message);
    }
    
    [TestMethod]
    public void TrashNotValidExceptionTest2()
    {
        // ARRANGE
        TrashNotValidException trashNotValidException = new TrashNotValidException(1);

        // ASSERT
        Assert.AreEqual("Trash with id 1 not found", trashNotValidException.Message);
    }
    
    [TestMethod]
    public void UserNotValidExceptionTest()
    {
        // ARRANGE
        UserNotValidException userNotValidException = new UserNotValidException("User not valid");

        // ASSERT
        Assert.AreEqual("User not valid", userNotValidException.Message);
    }
    
    [TestMethod]
    public void UserNotValidExceptionTest2()
    {
        // ARRANGE
        UserNotValidException userNotValidException = new UserNotValidException(1);

        // ASSERT
        Assert.AreEqual("User with id 1 not found", userNotValidException.Message);
    }
    
    [TestMethod]
    public void TeamNotValidExceptionTest()
    {
        // ARRANGE
        TeamNotValidException teamNotValidException = new TeamNotValidException("Team not valid");

        // ASSERT
        Assert.AreEqual("Team not valid", teamNotValidException.Message);
    }
    
    [TestMethod]
    public void TeamNotValidExceptionTest2()
    {
        // ARRANGE
        TeamNotValidException teamNotValidException = new TeamNotValidException(1);

        // ASSERT
        Assert.AreEqual("Team with id 1 not found", teamNotValidException.Message);
    }
    
    [TestMethod]
    public void TaskNotValidExceptionTest()
    {
        // ARRANGE
        TaskNotValidException taskNotValidException = new TaskNotValidException("Task not valid");

        // ASSERT
        Assert.AreEqual("Task not valid", taskNotValidException.Message);
    }
    
    [TestMethod]
    public void TaskNotValidExceptionTest2()
    {
        // ARRANGE
        TaskNotValidException taskNotValidException = new TaskNotValidException(1);

        // ASSERT
        Assert.AreEqual("Task with id 1 not found", taskNotValidException.Message);
    }
    
    [TestMethod]
    public void PanelNotValidExceptionTest()
    {
        // ARRANGE
        PanelNotValidException panelNotValidException = new PanelNotValidException("Panel not valid");

        // ASSERT
        Assert.AreEqual("Panel not valid", panelNotValidException.Message);
    }
    
    [TestMethod]
    public void PanelNotValidExceptionTest2()
    {
        // ARRANGE
        PanelNotValidException panelNotValidException = new PanelNotValidException(1);

        // ASSERT
        Assert.AreEqual("Panel with id 1 not found", panelNotValidException.Message);
    }
}