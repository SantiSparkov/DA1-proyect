using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.User;

namespace TaskPanelTest.ServiceTest;

[TestClass]
public class UserServiceTest
{
    private IUserService _userService;
    
    [TestInitialize]
    public void Initialize()
    {
        _userService = new UserService();
    }

    [TestCleanup]
    public void Cleanup()
    {
        var users = _userService.GetAllUsers();
        var usersToDelete = users.ToList();
        foreach (var user in usersToDelete)
        {
            _userService.DeleteUser(user.Id);
        }
    }

    [TestMethod]
    public User AddUser()
    {
        // Arrange
        var newUser = new User
        {
            Email = "user1@example.com",
            Name = "John",
            LastName = "Doe",
            BirthDate = DateTime.Now,
            Password = "StrongPass123#"
        };

        // Act
        var addedUser = _userService.AddUser(newUser);

        // Assert
        Assert.IsNotNull(addedUser);
        Assert.AreEqual("user1@example.com", addedUser.Email);
    }

    [TestMethod]
    public void UpdateUser()
    {
        // Arrange
        var existingUser = new User
        {
            Email = "user2@example.com",
            Name = "Jane",
            LastName = "Doe",
            BirthDate = DateTime.Now,
            Password = "OldPass123#"
        };

        _userService.AddUser(existingUser);

        var updatedUser = new User
        {
            Id = 1,
            Email = "user2@example.com",
            Name = "Jane Updated",
            LastName = "Doe Updated",
            BirthDate = DateTime.Now,
            Password = "NewPass123#"
        };

        // Act
        var result = _userService.UpdateUser(updatedUser);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Jane Updated", result.Name);
        Assert.AreEqual("Doe Updated", result.LastName);
    }

    [TestMethod]
    public void DeleteUser()
    {
        // Arrange
        var existingUser = new User
        {
            Email = "user3@example.com",
            Name = "Mark",
            LastName = "Smith",
            BirthDate = DateTime.Now,
            Password = "Pass123#"
        };

        var addedUser = _userService.AddUser(existingUser);

        // Act
        var deletedUser = _userService.DeleteUser(addedUser.Id);

        // Assert
        Assert.IsNotNull(deletedUser);
        Assert.AreEqual(addedUser.Id, deletedUser.Id);
        Assert.ThrowsException<UserNotFoundException>(new Action(() => _userService.GetUserById(addedUser.Id)));
    }


    [TestMethod]
    public void AddUser_ShouldThrowException_WhenEmailAlreadyExists()
    {
        // Arrange
        var existingUser = new User
        {
            Email = "admin@example.com", Name = "Admin", LastName = "Test", BirthDate = DateTime.Now,
            Password = "Admin123#"
        };

        _userService.AddUser(existingUser);

        var newUser = new User
        {
            Email = "admin@example.com", Name = "New", LastName = "User", BirthDate = DateTime.Now,
            Password = "User123#"
        };

        // Act & Assert
        Assert.ThrowsException<UserAlreadyExistsException>(new Action(() => _userService.AddUser(newUser)));
    }

    [TestMethod]
    public void AddUser_ShouldValidatePassword()
    {
        // Arrange
        var newUser = new User
        {
            Email = "user@example.com",
            Name = "John",
            LastName = "Doe",
            BirthDate = DateTime.Now,
            Password = "weakpass"
        };

        // Act & Assert
        Assert.ThrowsException<WeakPasswordException>(new Action(() => _userService.AddUser(newUser)));
    }
}