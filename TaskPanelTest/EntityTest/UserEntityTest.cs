using TaskPanelLibrary.Entity;

namespace TaskPanelTest.EntityTest;

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
            BirthDate = new DateTime(2008, 6, 1, 7, 47, 0),
            Password = "password",
            IsAdmin = false,
            Teams = new List<Team>(),
            TrashId = 1
        };
        
        //Act 
        
        // Assert
        Assert.AreEqual("Name", user.Name);
        Assert.AreEqual("LastName", user.LastName);
        Assert.AreEqual("email@email.com", user.Email);
        Assert.AreEqual(new DateTime(2008, 6, 1, 7, 47, 0), user.BirthDate);
        Assert.AreEqual("password", user.Password);
        Assert.AreEqual(1, user.TrashId);
        Assert.IsNotNull(user.Teams);
        Assert.IsFalse(user.IsAdmin);
    }
}