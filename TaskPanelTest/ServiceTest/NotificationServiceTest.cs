using Moq;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.Comment;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;

namespace TaskPanelTest.ServiceTest
{
    [TestClass]
    public class NotificationServiceTest
    {
        private Mock<INotificationRepository> _mockNotificationRepository;
        
        private Mock<IAuthService> _mockAuthService;
        
        private NotificationService _notificationService;

        [TestInitialize]
        public void Initialize()
        {
            _mockNotificationRepository = new Mock<INotificationRepository>();
            _mockAuthService = new Mock<IAuthService>();
            _notificationService = new NotificationService(_mockNotificationRepository.Object, _mockAuthService.Object);
        }

        [TestMethod]
        public void CreateNotification_ShouldAddNotification()
        {
            // Arrange
            var currentUser = new User { Id = 1, Name = "John", Email = "john@example.com" };
            _mockAuthService.Setup(a => a.GetCurrentUser()).Returns(currentUser);

            // Act
            var notification = _notificationService.CreateNotification(123, "Task resolved");

            // Assert
            _mockNotificationRepository.Verify(
                n => n.CreateNotification(It.Is<Notification>(not =>
                    not.Message == "Comment #123 has been resolved. Message: Task resolved" &&
                    not.UserId == 1 &&
                    not.User == currentUser)),
                Times.Once
            );
        }

        [TestMethod]
        public void CreateNotification_ShouldThrowExceptionForEmptyMessage()
        {
            // Act & Assert
            var exception = Assert.ThrowsException<NotificationNotValidException>(
                () => _notificationService.CreateNotification(123, ""));
            Assert.AreEqual("Notification can not be empty.", exception.Message);
        }

        [TestMethod]
        public void GetNotifications_ShouldReturnNotifications()
        {
            // Arrange
            var notifications = new List<Notification>
            {
                new Notification { Id = 1, Message = "First notification", UserId = 1 },
                new Notification { Id = 2, Message = "Second notification", UserId = 1 }
            };
            _mockNotificationRepository.Setup(n => n.GetNotifications(1)).Returns(notifications);

            // Act
            var result = _notificationService.GetNotifications(1);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("First notification", result[0].Message);
            Assert.AreEqual("Second notification", result[1].Message);
        }

        [TestMethod]
        public void DeleteNotification_ShouldRemoveNotification()
        {
            // Act
            _notificationService.DeleteNotification(1);

            // Assert
            _mockNotificationRepository.Verify(n => n.DeleteNotification(1), Times.Once);
        }

        [TestMethod]
        public void CreateNotification_ShouldTriggerOnNotificationAdded()
        {
            // Arrange
            var currentUser = new User { Id = 1, Name = "John", Email = "john@example.com" };
            _mockAuthService.Setup(a => a.GetCurrentUser()).Returns(currentUser);

            bool eventTriggered = false;
            _notificationService.OnNotificationAdded = () => eventTriggered = true;

            // Act
            _notificationService.CreateNotification(123, "Task resolved");

            // Assert
            Assert.IsTrue(eventTriggered, "The OnNotificationAdded event was not triggered.");
        }

        [TestMethod]
        public void DeleteNotification_ShouldTriggerOnNotificationAdded()
        {
            // Arrange
            bool eventTriggered = false;
            _notificationService.OnNotificationAdded = () => eventTriggered = true;

            // Act
            _notificationService.DeleteNotification(1);

            // Assert
            Assert.IsTrue(eventTriggered, "The OnNotificationAdded event was not triggered.");
        }
    }
}
