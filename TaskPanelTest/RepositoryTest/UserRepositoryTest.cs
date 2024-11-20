using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.User;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;

namespace TaskPanelTest.RepositoryTest;

[TestClass]
public class UserRepositoryTests
{
    private IUserRepository _userRepository;

    [TestInitialize]
    public void Initialize()
    {
        _userRepository = new UserRepository();
    }

    [TestCleanup]
    public void Cleanup()
    {
        var users = _userRepository.GetAllUsers();
        var usersToDelete = users.ToList();
        foreach (var user in usersToDelete)
        {
            _userRepository.DeleteUser(user.Id);
        }
    }

    [TestMethod]
    public void AddUser()
    {
        // Arrange
        var user = new User
        {
            Name = "John", Email = "john.doe@example.com", LastName = "Doe", BirthDate = new DateTime(1980, 5, 1),
            IsAdmin = false
        };

        // Act
        _userRepository.AddUser(user);

        // Assert
        var actualUser = _userRepository.GetUserById(user.Id);
        Assert.IsNotNull(actualUser, "The user was not added to the repository.");
        Assert.AreEqual("John", actualUser.Name, "The user's name was not stored correctly.");
    }

    [TestMethod]
    public void DeleteUser()
    {
        // Arrange
        var user = new User
        {
            Name = "John", Email = "john.doe@example.com", LastName = "Doe", BirthDate = new DateTime(1980, 5, 1),
            IsAdmin = false
        };
        _userRepository.AddUser(user);

        // Act
        _userRepository.DeleteUser(user.Id);

        // Assert
        var actualUser = _userRepository.GetUserById(user.Id);
        Assert.IsNull(actualUser, "The user was not deleted from the repository.");
    }

    [TestMethod]
    public void UpdateUser()
    {
        // Arrange
        var user = new User
        {
            Name = "John", Email = "john.doe@example.com", LastName = "Doe", BirthDate = new DateTime(1980, 5, 1),
            IsAdmin = false
        };
        var addedUser = _userRepository.AddUser(user);

        // Act
        var updatedUser = new User
            { Id = addedUser.Id, Name = "John Updated", LastName = "Doe Updated", Email = user.Email };
        _userRepository.UpdateUser(updatedUser);

        // Assert
        var actualUser = _userRepository.GetUserById(user.Id);
        Assert.IsNotNull(actualUser, "The user was not found after update.");
        Assert.AreEqual("John Updated", actualUser.Name, "The user's name was not updated correctly.");
        Assert.AreEqual("Doe Updated", actualUser.LastName, "The user's last name was not updated correctly.");
    }

    [TestMethod]
    public void GetUserByEmail()
    {
        // Arrange
        var user1 = new User
        {
            Name = "John", Email = "john.doe@example.com", LastName = "Doe", BirthDate = new DateTime(1980, 5, 1),
            IsAdmin = false
        };
        var user2 = new User
        {
            Name = "Jane", Email = "jane.doe@example.com", LastName = "Doe", BirthDate = new DateTime(1985, 6, 1),
            IsAdmin = false
        };
        _userRepository.AddUser(user1);

        // Act
        var actualUser = _userRepository.AddUser(user2);

        // Assert
        Assert.IsNotNull(actualUser, "The user was not found.");
        Assert.AreEqual("Jane", actualUser.Name, "The user's name does not match.");
    }

    [TestMethod]
    public void GetAllUsers()
    {
        // Arrange
        var user1 = new User
        {
            Name = "John", Email = "john.doe@example.com", LastName = "Doe", BirthDate = new DateTime(1980, 5, 1),
            IsAdmin = false
        };
        var user2 = new User
        {
            Name = "Jane", Email = "jane.doe@example.com", LastName = "Doe", BirthDate = new DateTime(1985, 6, 1),
            IsAdmin = false
        };
        _userRepository.AddUser(user1);
        _userRepository.AddUser(user2);

        // Act
        var allUsers = _userRepository.GetAllUsers();

        // Assert
        Assert.AreEqual(3, allUsers.Count, "The number of users in the repository does not match.");
    }

    [TestMethod]
    public void UpdateUser_ShouldThrowExceptionIfUserDoesNotExist()
    {
        // Arrange
        var user = new User { Name = "Non Existent", Email = "non.existent@example.com" };

        // Act & Assert
        var exception =
            Assert.ThrowsException<UserNotValidException>(new Action(() => _userRepository.UpdateUser(user)));
        Assert.AreEqual("non.existent@example.com", exception.Message);
    }
}