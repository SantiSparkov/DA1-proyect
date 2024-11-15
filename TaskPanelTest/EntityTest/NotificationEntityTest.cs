using TaskPanelLibrary.Entity;

namespace TaskPanelTest.EntityTest
{
    [TestClass]
    public class NotificationEntityTest
    {
        [TestMethod]
        public void CreateNotificationTest()
        {
            // Arrange
            Notification notification = new Notification()
            {
                Id = 1,
                Message = "New task assigned",
                UserId = 123,
                User = new User { Id = 123, Name = "John", Email = "john@example.com" }
            };

            // Assert
            Assert.AreEqual(1, notification.Id);
            Assert.AreEqual("New task assigned", notification.Message);
            Assert.AreEqual(123, notification.UserId);
            Assert.IsNotNull(notification.User);
            Assert.AreEqual(123, notification.User.Id);
            Assert.AreEqual("John", notification.User.Name);
            Assert.AreEqual("john@example.com", notification.User.Email);
        }
    }
}