using Moq;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Repository;
using TaskPanelLibrary.Repository.Interface;
using TaskPanelLibrary.Config;
using TaskPanelTest.ConfigTest;

namespace TaskPanelTest.RepositoryTest.SqlRepositories
{
    [TestClass]
    public class NotificationSqlRepositoryTests
    {
        private SqlContext _sqlContext;

        private INotificationRepository _notificationRepository;
    
        [TestInitialize]
        public void Initialize()
        {
            _sqlContext = new SqlContexTest().CreateMemoryContext();
            _notificationRepository = new NotificationSqlRepository(_sqlContext);
        }
    
        [TestCleanup]
        public void CleanUp()
        {
            _sqlContext?.Database.EnsureDeleted();
        }

        [TestMethod]
        public void CreateNotification()
        {
            // Arrange
            Notification notification = new Notification
            {
                Id = 1,
                Message = "You have a new task assigned",
                UserId = 1,
                User = new User { Id = 1, Name = "John", Email = "john@example.com", LastName = "LastName", Password = "password"}
            };

            // Act
            _notificationRepository.CreateNotification(notification);

            List<Notification> notificationsForUser = _notificationRepository.GetNotifications(notification.UserId);

            // Assert
            Assert.AreEqual(1, notificationsForUser.Count);
        }

        [TestMethod]
        public void DeleteNotification()
        {
            // Arrange
            Notification notification = new Notification { Id = 1, Message = "To Delete", UserId = 1 };

            // Act
            _notificationRepository.CreateNotification(notification);

            _notificationRepository.DeleteNotification(notification.Id);
            List<Notification> notificationsForUser = _notificationRepository.GetNotifications(notification.UserId);

            // Assert
            Assert.AreEqual(0, notificationsForUser.Count);

        }
    }
}
