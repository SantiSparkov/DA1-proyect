using Moq;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using TaskPanelLibrary.Config;

namespace TaskPanelTest.RepositoryTest.SqlRepositories
{
    [TestClass]
    public class NotificationSqlRepositoryTests
    {
        private Mock<DbSet<Notification>> _mockNotificationSet;
        private Mock<SqlContext> _mockContext;
        private INotificationRepository _notificationRepository;

        [TestInitialize]
        public void Initialize()
        {
            _mockNotificationSet = new Mock<DbSet<Notification>>();
            _mockContext = new Mock<SqlContext>();
            _mockContext.Setup(c => c.Notifications).Returns(_mockNotificationSet.Object);
            _notificationRepository = new NotificationSqlRepository(_mockContext.Object);
        }

        [TestMethod]
        public void CreateNotification_ShouldAddNotification()
        {
            // Arrange
            var notification = new Notification
            {
                Id = 1,
                Message = "You have a new task assigned",
                UserId = 1,
                User = new User { Id = 1, Name = "John", Email = "john@example.com" }
            };

            // Act
            _notificationRepository.CreateNotification(notification);

            // Assert
            _mockNotificationSet.Verify(n => n.Add(It.Is<Notification>(not => not == notification)), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void GetNotifications_ShouldReturnNotificationsForUser()
        {
            // Arrange
            var notifications = new List<Notification>
            {
                new Notification { Id = 1, Message = "First", UserId = 1 },
                new Notification { Id = 2, Message = "Second", UserId = 1 },
                new Notification { Id = 3, Message = "Other", UserId = 2 }
            }.AsQueryable();

            _mockNotificationSet.As<IQueryable<Notification>>().Setup(m => m.Provider).Returns(notifications.Provider);
            _mockNotificationSet.As<IQueryable<Notification>>().Setup(m => m.Expression).Returns(notifications.Expression);
            _mockNotificationSet.As<IQueryable<Notification>>().Setup(m => m.ElementType).Returns(notifications.ElementType);
            _mockNotificationSet.As<IQueryable<Notification>>().Setup(m => m.GetEnumerator()).Returns(notifications.GetEnumerator());

            // Act
            var result = _notificationRepository.GetNotifications(1);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.All(n => n.UserId == 1));
        }

        [TestMethod]
        public void DeleteNotification_ShouldRemoveNotification()
        {
            // Arrange
            var notification = new Notification { Id = 1, Message = "To Delete", UserId = 1 };
            _mockNotificationSet.Setup(m => m.Find(It.IsAny<int>())).Returns(notification);

            // Act
            _notificationRepository.DeleteNotification(notification.Id);

            // Assert
            _mockNotificationSet.Verify(n => n.Remove(It.Is<Notification>(not => not == notification)), Times.Once);
            _mockContext.Verify(c => c.SaveChanges(), Times.Once);
        }
    }
}
