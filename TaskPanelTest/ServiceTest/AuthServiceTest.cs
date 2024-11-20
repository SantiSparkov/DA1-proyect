using Moq;
using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;

namespace TaskPanelTest.ServiceTest
{
    [TestClass]
    public class AuthServiceTests
    {
        private Mock<IUserService> _userServiceMock;
        private IAuthService _authService;
        private List<User> _users;

        [TestInitialize]
        public void Setup()
        {
            _userServiceMock = new Mock<IUserService>();

            _users = new List<User>
            {
                new User { Id = 1, Email = "admin@taskpanel.com", Password = "admin123", IsAdmin = true },
                new User { Id = 2, Email = "user@taskpanel.com", Password = "user123", IsAdmin = false }
            };

            _userServiceMock.Setup(service => service.GetAllUsers()).Returns(_users);

            _authService = new AuthService(_userServiceMock.Object);
        }

        [TestMethod]
        public void Login_WithValidCredentials_ShouldReturnTrue()
        {
            var result = _authService.Login("admin@taskpanel.com", "admin123");

            Assert.IsTrue(result);
            Assert.AreEqual("admin@taskpanel.com", _authService.GetCurrentUser().Email);
            Assert.AreEqual("admin123", _authService.GetCurrentUser().Password);
        }

        [TestMethod]
        public void Login_WithInvalidCredentials_ShouldReturnFalse()
        {
            var result = _authService.Login("admin@taskpanel.com", "wrongpassword");

            Assert.IsFalse(result);
            Assert.IsNull(_authService.GetCurrentUser());
        }

        [TestMethod]
        public void IsLoggedIn_ShouldReturnTrue_WhenUserIsLoggedIn()
        {
            _authService.Login("admin@taskpanel.com", "admin123");

            Assert.IsTrue(_authService.IsLoggedIn());
        }

        [TestMethod]
        public void IsLoggedIn_ShouldReturnFalse_WhenNoUserIsLoggedIn()
        {
            Assert.IsFalse(_authService.IsLoggedIn());
        }

        [TestMethod]
        public void Logout_ShouldClearCurrentUser()
        {
            _authService.Login("admin@taskpanel.com", "admin123");
            _authService.Logout();

            Assert.IsNull(_authService.GetCurrentUser());
            Assert.IsFalse(_authService.IsLoggedIn());
        }
    }
}