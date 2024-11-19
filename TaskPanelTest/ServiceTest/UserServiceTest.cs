/*using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.User;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;

namespace TaskPanelTest.ServiceTest;


public class UserServiceTest
{
    private IUserService _userService;

    private IUserRepository _userRepository;

    private PasswordGeneratorService _passwordGenerator;

    [TestInitialize]
    public void Initialize()
    {
        _userRepository = new UserRepository();
        _passwordGenerator = new PasswordGeneratorService();
        _userService = new UserService(_userRepository, _passwordGenerator);
    }

    [TestCleanup]
    public void Cleanup()
    {
        var users = _userService.GetAllUsers().ToList();
        foreach (var user in users)
        {
            _userService.DeleteUser(user.Id);
        }
    }

    [TestMethod]
    public void AddUser()
    {
        // Arrange
        var newUser = new User
        {
            Email = "user1@example.com",
            Name = "John",
            LastName = "Doe",
            BirthDate = DateTime.Now,
        };

        // Act
        var addedUser = _userService.AddUser(newUser);

        // Assert
        Assert.IsNotNull(addedUser);
        Assert.AreEqual("user1@example.com", addedUser.Email);
    }

    [TestMethod]
    public void AddUser_NotValid()
    {
        // Arrange
        var newUser = new User
        {
            BirthDate = DateTime.Now,
        };
        
        // Act & Assert
        Assert.ThrowsException<UserNotValidException>(new Action(() => _userService.AddUser(newUser)));
        Assert.AreEqual(1, _userService.GetAllUsers().Count());
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
        };

        _userService.AddUser(existingUser);

        var updatedUser = new User
        {
            Id = existingUser.Id,
            Email = "user2@example.com",
            Name = "Jane Updated",
            LastName = "Doe Updated"
        };

        // Act
        var result = _userService.UpdateUser(updatedUser);

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Jane Updated", result.Name);
        Assert.AreEqual("Doe Updated", result.LastName);
    }

    [TestMethod]
    public void UpdateUserNonExistingUser()
    {
        // Arrange
        var nonExistingUser = new User
        {
            Id = 2,
            Email = "user2@example.com",
            Name = "Jane",
            LastName = "Doe",
            BirthDate = DateTime.Now,
        };

        // Act & Assert
        Assert.ThrowsException<UserNotValidException>(new Action(() => _userService.UpdateUser(nonExistingUser)));
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
        };

        var addedUser = _userService.AddUser(existingUser);

        // Act
        var deletedUser = _userService.DeleteUser(addedUser.Id);

        // Assert
        Assert.IsNotNull(deletedUser);
        Assert.AreEqual(addedUser.Id, deletedUser.Id);
        Assert.ThrowsException<UserNotValidException>(new Action(() => _userService.GetUserById(addedUser.Id)));
    }

    [TestMethod]
    public void DeteteNonExistingUser()
    {
        // Arrange
        var newUser = new User
        {
            Email = "admin@example.com", Name = "New", LastName = "User", BirthDate = DateTime.Now
        };

        var addedUser = _userService.AddUser(newUser);

        // Act & Assert
        Assert.ThrowsException<UserNotValidException>(new Action(() => _userService.DeleteUser(addedUser.Id + 1)));
    }


    [TestMethod]
    public void AddUser_ShouldThrowException_WhenEmailAlreadyExists()
    {
        // Arrange
        var existingUser = new User
        {
            Email = "admin@example.com", Name = "Admin", LastName = "Test", BirthDate = DateTime.Now
        };

        _userService.AddUser(existingUser);

        var newUser = new User
        {
            Email = "admin@example.com", Name = "New", LastName = "User", BirthDate = DateTime.Now
        };

        // Act & Assert
        Assert.ThrowsException<UserNotValidException>(new Action(() => _userService.AddUser(newUser)));
    }
}*/