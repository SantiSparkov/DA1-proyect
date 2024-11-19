using Moq;
using TaskPanel.Pages.User;
using TaskPanelLibrary.Config;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service.Interface;
using TaskPanelTest.ConfigTest;

namespace TaskPanelTest.RepositoryTest.SqlRepositories;

[TestClass]
public class UserSqlRepositoryTest
{

    private SqlContext _sqlContext;

    private Mock<ITrashService> _trashService;

    private IUserRepository _userRepository;
    
    [TestInitialize]
    public void Initialize()
    {
        _sqlContext = new SqlContexTest().CreateMemoryContext();
        _trashService = new Mock<ITrashService>();
        _userRepository = new UserSqlRepository(_sqlContext, _trashService.Object);
    }
    
    [TestCleanup]
    public void CleanUp()
    {
        _sqlContext?.Database.EnsureDeleted();
    }
    
    [TestMethod]
    public void AddTrash()
    {
        //Arrange
        User user = new User
        {
            Name = "Admin",
            LastName = "User",
            Email = "admin@admin.com",
            Password = "Aa1@",
            IsAdmin = true,
            BirthDate = new DateTime(1990, 1, 1)
        };
        
        //Act
        _userRepository.AddUser(user);

        //Assert
        Assert.AreEqual(2, _userRepository.GetAllUsers().Count);
    }
    
    [TestMethod]
    public void DeleteUser()
    {
        //Arrange
        User user = new User
        {
            Name = "Admin",
            LastName = "User",
            Email = "admin@admin.com",
            Password = "Aa1@",
            IsAdmin = true,
            BirthDate = new DateTime(1990, 1, 1)
        };
        
        //Act
        _userRepository.AddUser(user);
        User userDelete = _userRepository.DeleteUser(user.Id);

        //Assert
        Assert.AreEqual(user.Id, userDelete.Id);
        Assert.AreEqual(user.Email, userDelete.Email);
        Assert.AreEqual(user.Name, userDelete.Name);
        Assert.AreEqual(user.BirthDate, userDelete.BirthDate);
        Assert.IsTrue(user.IsAdmin);
    }
    
    [TestMethod]
    public void DeleteUserNotExist()
    {
        //Arrange
        
        //Act
        var exception = Assert.ThrowsException<System.Exception>(() => _userRepository.DeleteUser(2));

        //Assert
        Assert.AreEqual($"User with id: {2} does not exist", exception.Message);
    }
    
    [TestMethod]
    public void GetUserById()
    {
        //Arrange
        User user = new User
        {
            Name = "Admin",
            LastName = "User",
            Email = "admin@admin.com",
            Password = "Aa1@",
            IsAdmin = true,
            BirthDate = new DateTime(1990, 1, 1)
        };
        
        //Act
        _userRepository.AddUser(user);
        
        User userSaved = _userRepository.GetUserById(user.Id);

        //Assert
        Assert.AreEqual(user.Id, userSaved.Id);
        Assert.AreEqual(user.Email, userSaved.Email);
        Assert.AreEqual(user.Name, userSaved.Name);
        Assert.AreEqual(user.BirthDate, userSaved.BirthDate);
        Assert.IsTrue(userSaved.IsAdmin);
    }
    
    [TestMethod]
    public void GetUserByIdNotExist()
    {
        //Arrange
        
        //Act
        var exception = Assert.ThrowsException<System.Exception>(() => _userRepository.GetUserById(2));

        //Assert
        Assert.AreEqual($"User with id: {2} does not exist", exception.Message);
    }
    
    [TestMethod]
    public void GetAllUsers()
    {
        //Arrange
        User user = new User
        {
            Name = "Admin",
            LastName = "User",
            Email = "admin@admin.com",
            Password = "Aa1@",
            IsAdmin = true,
            BirthDate = new DateTime(1990, 1, 1)
        };
        
        //Act
        _userRepository.AddUser(user);
        List<User> users = _userRepository.GetAllUsers();

        //Assert
        Assert.AreEqual(2,users.Count);
    }
    
    [TestMethod]
    public void UpdateUser()
    {
        //Arrange
        User user = new User
        {
            Name = "Admin",
            LastName = "User",
            Email = "admin@admin.com",
            Password = "Aa1@",
            IsAdmin = true,
            BirthDate = new DateTime(1990, 1, 1)
        };
        
        //Act
        _userRepository.AddUser(user);

        user.Name = "Updated";
        user.LastName = "LastName updated";
        User userSaved = _userRepository.UpdateUser(user);

        //Assert
        Assert.AreEqual("LastName updated", userSaved.LastName);
        Assert.AreEqual("Updated", userSaved.Name);
    }
}