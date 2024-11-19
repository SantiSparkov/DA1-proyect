using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.User;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;
using System;
using System.Collections.Generic;

namespace TaskPanelTest.ServiceTest
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<ITrashService> _mockTrashService;
        private UserService _userService;

        [TestInitialize]
        public void Setup()
        {
            _mockUserRepository = new Mock<IUserRepository>();
            _mockTrashService = new Mock<ITrashService>();
            _userService = new UserService(_mockUserRepository.Object, _mockTrashService.Object);
        }

        [TestMethod]
        public void GetAllUsers_ShouldReturnAllUsers()
        {
            // Arrange
            var users = new List<User>
            {
                new User { Id = 1, Email = "user1@test.com" },
                new User { Id = 2, Email = "user2@test.com" }
            };
            _mockUserRepository.Setup(repo => repo.GetAllUsers()).Returns(users);

            // Act
            var result = _userService.GetAllUsers();

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public void GetUserById_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Email = "user@test.com" };
            _mockUserRepository.Setup(repo => repo.GetUserById(1)).Returns(user);

            // Act
            var result = _userService.GetUserById(1);

            // Assert
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(user.Email, result.Email);
        }

        [TestMethod]
        public void GetUserById_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUserById(It.IsAny<int>())).Returns((User)null);

            // Act & Assert
            Assert.ThrowsException<UserNotValidException>(() => _userService.GetUserById(1));
        }

        [TestMethod]
        public void AddUser_ShouldAddUser_WhenUserIsValidAndDoesNotExist()
        {
            // Arrange
            var user = new User
            {
                Id = 1, Email = "user@test.com", Name = "Test", LastName = "User",
                BirthDate = DateTime.Now.AddYears(-25)
            };
            var trash = new Trash { Id = 1 };
            _mockUserRepository.Setup(repo => repo.GetAllUsers()).Returns(new List<User>());
            _mockUserRepository.Setup(repo => repo.AddUser(It.IsAny<User>())).Returns(user);
            _mockTrashService.Setup(service => service.CreateTrash(user)).Returns(trash);

            // Act
            var result = _userService.AddUser(user);

            // Assert
            Assert.AreEqual(user.Id, result.Id);
            Assert.AreEqual(trash.Id, result.TrashId);
        }

        [TestMethod]
        public void AddUser_ShouldThrowException_WhenUserAlreadyExists()
        {
            // Arrange
            var existingUser = new User { Email = "user@test.com" };
            var user = new User { Email = "user@test.com" };
            _mockUserRepository.Setup(repo => repo.GetAllUsers()).Returns(new List<User> { existingUser });

            // Act & Assert
            Assert.ThrowsException<UserNotValidException>(() => _userService.AddUser(user));
        }

        [TestMethod]
        public void AddUser_ShouldThrowException_WhenUserIsInvalid()
        {
            // Arrange
            var user = new User
            {
                Email = "",
                Name = "",
                LastName = "",
                BirthDate = new DateTime(1900, 12, 31)
            };

            var trash = new Trash { Id = 1 };
            _mockUserRepository.Setup(repo => repo.GetAllUsers()).Returns(new List<User>());
            _mockTrashService.Setup(service => service.CreateTrash(It.IsAny<User>())).Returns(new Trash());

            // Act & Assert
            Assert.ThrowsException<UserNotValidException>(() => _userService.AddUser(user));
        }

        [TestMethod]
        public void UpdateUser_ShouldUpdateUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, Email = "user@test.com" };
            _mockUserRepository.Setup(repo => repo.GetUserById(1)).Returns(user);
            _mockUserRepository.Setup(repo => repo.UpdateUser(It.IsAny<User>())).Returns(user);

            // Act
            var result = _userService.UpdateUser(user);

            // Assert
            Assert.AreEqual(user.Id, result.Id);
        }

        [TestMethod]
        public void UpdateUser_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var user = new User { Id = 1, Email = "user@test.com" };
            _mockUserRepository.Setup(repo => repo.GetUserById(1)).Returns((User)null);

            // Act & Assert
            Assert.ThrowsException<UserNotValidException>(() => _userService.UpdateUser(user));
        }

        [TestMethod]
        public void DeleteUser_ShouldDeleteUser_WhenUserExists()
        {
            // Arrange
            var user = new User { Id = 1, TrashId = 1 };
            _mockUserRepository.Setup(repo => repo.GetUserById(1)).Returns(user);
            _mockUserRepository.Setup(repo => repo.DeleteUser(1)).Returns(user);
            _mockTrashService.Setup(service => service.DeleteTrash(user.TrashId));

            // Act
            var result = _userService.DeleteUser(1);

            // Assert
            Assert.AreEqual(user.Id, result.Id);
            _mockTrashService.Verify(service => service.DeleteTrash(user.TrashId), Times.Once);
        }

        [TestMethod]
        public void DeleteUser_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            _mockUserRepository.Setup(repo => repo.GetUserById(It.IsAny<int>())).Returns((User)null);

            // Act & Assert
            Assert.ThrowsException<UserNotValidException>(() => _userService.DeleteUser(1));
        }

        [TestMethod]
        public void AddUser_ShouldThrowException_WhenBirthDateIsInFuture()
        {
            // Arrange
            var user = new User
            {
                Email = "user@test.com",
                Name = "Old",
                LastName = "User",
                BirthDate = new DateTime(2100, 12, 31)
            };

            var trash = new Trash { Id = 1 };
            _mockUserRepository.Setup(repo => repo.GetAllUsers()).Returns(new List<User>());
            _mockTrashService.Setup(service => service.CreateTrash(It.IsAny<User>())).Returns(new Trash());

            // Act & Assert
            Assert.ThrowsException<UserNotValidException>(() => _userService.AddUser(user));
        }

        [TestMethod]
        public void AddUser_ShouldThrowException_WhenBirthDateIsBefore1900()
        {
            // Arrange
            var user = new User
            {
                Email = "user@test.com",
                Name = "Old",
                LastName = "User",
                BirthDate = new DateTime(1899, 12, 31)
            };

            var trash = new Trash { Id = 1 };
            _mockUserRepository.Setup(repo => repo.GetAllUsers()).Returns(new List<User>());
            _mockTrashService.Setup(service => service.CreateTrash(It.IsAny<User>())).Returns(new Trash());

            // Act & Assert
            Assert.ThrowsException<UserNotValidException>(() => _userService.AddUser(user));
        }

        [TestMethod]
        public void AddUser_ShouldThrowException_WhenUserLastNameIsEmpty()
        {
            // Arrange
            var user = new User
            {
                Email = "user@test.com",
                Name = "Old",
                LastName = "",
                BirthDate = new DateTime(1999, 12, 31),
            };

            var trash = new Trash { Id = 1 };
            _mockUserRepository.Setup(repo => repo.GetAllUsers()).Returns(new List<User>());
            _mockTrashService.Setup(service => service.CreateTrash(It.IsAny<User>())).Returns(new Trash());

            // Act & Assert
            Assert.ThrowsException<UserNotValidException>(() => _userService.AddUser(user));
        }

        [TestMethod]
        public void AddUser_ShouldThrowException_WhenUserIsNull()
        {
            // Arrange
            User user = null;
            
            var trash = new Trash { Id = 1 };
            _mockUserRepository.Setup(repo => repo.GetAllUsers()).Returns(new List<User>());
            _mockTrashService.Setup(service => service.CreateTrash(It.IsAny<User>())).Returns(new Trash());
            
            // Act & Assert
            Assert.ThrowsException<UserNotValidException>(() => _userService.AddUser(user));
        }
    }
}