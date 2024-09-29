namespace TaskPanelTest.RepositoryTest.UserRepositoryTest;

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
        foreach (var user in users)
        {
            _userRepository.DeleteUser(user.Email);
        }
    }

    [TestMethod]
    public void AddUser()
    {
        // Arrange
        var user = new RUser { Name = "John", Email = "john.doe@example.com", LastName = "Doe", BrithDate = new DateTime(1980, 5, 1), IsAdmin = false };

        // Act
        _userRepository.AddUser(user);

        // Assert
        var actualUser = _userRepository.GetUserByEmail(user.Email);
        Assert.IsNotNull(actualUser, "The user was not added to the repository.");
        Assert.AreEqual("John", actualUser.Name, "The user's name was not stored correctly.");
    }

    [TestMethod]
    public void DeleteUser()
    {
        // Arrange
        var user = new RUser { Name = "John", Email = "john.doe@example.com", LastName = "Doe", BrithDate = new DateTime(1980, 5, 1), IsAdmin = false };
        _userRepository.AddUser(user);

        // Act
        _userRepository.DeleteUser(user.Email);

        // Assert
        var actualUser = _userRepository.GetUserByEmail(user.Email);
        Assert.IsNull(actualUser, "The user was not deleted from the repository.");
    }

    [TestMethod]
    public void UpdateUser()
    {
        // Arrange
        var user = new RUser { Name = "John", Email = "john.doe@example.com", LastName = "Doe", BrithDate = new DateTime(1980, 5, 1), IsAdmin = false };
        _userRepository.AddUser(user);

        // Act
        var updatedUser = new RUser { Name = "John Updated", LastName = "Doe Updated", Email = user.Email };
        _userRepository.UpdateUser(updatedUser);

        // Assert
        var actualUser = _userRepository.GetUserByEmail(user.Email);
        Assert.IsNotNull(actualUser, "The user was not found after update.");
        Assert.AreEqual("John Updated", actualUser.Name, "The user's name was not updated correctly.");
        Assert.AreEqual("Doe Updated", actualUser.LastName, "The user's last name was not updated correctly.");
    }

    [TestMethod]
    public void GetUserByEmail()
    {
        // Arrange
        var user1 = new RUser { Name = "John", Email = "john.doe@example.com", LastName = "Doe", BrithDate = new DateTime(1980, 5, 1), IsAdmin = false };
        var user2 = new RUser { Name = "Jane", Email = "jane.doe@example.com", LastName = "Doe", BrithDate = new DateTime(1985, 6, 1), IsAdmin = false };
        _userRepository.AddUser(user1);
        _userRepository.AddUser(user2);

        // Act
        var actualUser = _userRepository.GetUserByEmail("jane.doe@example.com");

        // Assert
        Assert.IsNotNull(actualUser, "The user was not found.");
        Assert.AreEqual("Jane", actualUser.Name, "The user's name does not match.");
    }

    [TestMethod]
    public void GetAllUsers()
    {
        // Arrange
        var user1 = new RUser { Name = "John", Email = "john.doe@example.com", LastName = "Doe", BrithDate = new DateTime(1980, 5, 1), IsAdmin = false };
        var user2 = new RUser { Name = "Jane", Email = "jane.doe@example.com", LastName = "Doe", BrithDate = new DateTime(1985, 6, 1), IsAdmin = false };
        _userRepository.AddUser(user1);
        _userRepository.AddUser(user2);

        // Act
        var allUsers = _userRepository.GetAllUsers();

        // Assert
        Assert.AreEqual(2, allUsers.Count, "The number of users in the repository does not match.");
    }
    
    [TestMethod]
    public void UpdateUser_ShouldThrowExceptionIfUserDoesNotExist()
    {
        // Arrange
        var user = new RUser { Name = "Non Existent", Email = "non.existent@example.com" };

        // Act & Assert
        var exception = Assert.ThrowsException<UserNotFoundException>(new Action(() => _userRepository.UpdateUser(user)));
        Assert.AreEqual("User with email non.existent@example.com does not exist.", exception.Message);
    }

}
