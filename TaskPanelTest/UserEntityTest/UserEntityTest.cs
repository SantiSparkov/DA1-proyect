using TaskPanel.Models.Entity;

namespace TaskPanelTest.UserEntityTest;

[TestClass]
public class UserEntityTest
{
    [TestMethod]
    public void CreateUserTest()
    {
        //Arrange 
        User user = new User()
        {
            Name = "Name",
            LastName = "LastName",
            Email = "email@email.com",
            BrithDate = new DateTime(2008, 6, 1, 7, 47, 0),
            Password = "password",
            IsAdmin = false,
            Trash = new Trash()
        };
        
        //Act 
        
        // Assert
        Assert.AreEqual("Name", user.Name);
        Assert.AreEqual("LastName", user.LastName);
        Assert.AreEqual("email@email.com", user.Email);
        Assert.AreEqual(new DateTime(2008, 6, 1, 7, 47, 0), user.BrithDate);
        Assert.AreEqual("password", user.Password);
        Assert.IsFalse(user.IsAdmin);
        Assert.IsNotNull(user.Trash);
    }
}