using System.ComponentModel.DataAnnotations;
using TaskPanelLibrary.Entity;

namespace TaskPanelTest.EntityTest
{
    [TestClass]
    public class LoginTests
    {
        private Login _login;

        [TestInitialize]
        public void Setup()
        {
            _login = new Login();
        }

        [TestMethod]
        public void Login_WithValidData_ShouldPassValidation()
        {
            // Arrange
            _login.Email = "test@example.com";
            _login.Password = "ValidPassword";

            // Act
            var results = new List<ValidationResult>();
            var context = new ValidationContext(_login);
            bool isValid = Validator.TryValidateObject(_login, context, results, true);

            // Assert
            Assert.IsTrue(isValid, "Expected no validation errors for valid data.");
        }

        [TestMethod]
        public void Login_WithoutEmail_ShouldFailValidation()
        {
            // Arrange
            _login.Email = "";
            _login.Password = "ValidPassword";

            // Act
            var results = new List<ValidationResult>();
            var context = new ValidationContext(_login);
            bool isValid = Validator.TryValidateObject(_login, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual("Email must not be empty.", results[0].ErrorMessage);
        }

        [TestMethod]
        public void Login_WithInvalidEmailFormat_ShouldFailValidation()
        {
            // Arrange
            _login.Email = "invalid-email";
            _login.Password = "ValidPassword";

            // Act
            var results = new List<ValidationResult>();
            var context = new ValidationContext(_login);
            bool isValid = Validator.TryValidateObject(_login, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual("Invalid email format.", results[0].ErrorMessage);
        }

        [TestMethod]
        public void Login_WithoutPassword_ShouldFailValidation()
        {
            // Arrange
            _login.Email = "test@example.com";
            _login.Password = "";

            // Act
            var results = new List<ValidationResult>();
            var context = new ValidationContext(_login);
            bool isValid = Validator.TryValidateObject(_login, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual("Password must not be empty.", results[0].ErrorMessage);
        }

        [TestMethod]
        public void Login_WithInvalidEmailPattern_ShouldFailValidation()
        {
            // Arrange
            _login.Email = "invalid@domain";
            _login.Password = "ValidPassword";

            // Act
            var results = new List<ValidationResult>();
            var context = new ValidationContext(_login);
            bool isValid = Validator.TryValidateObject(_login, context, results, true);

            // Assert
            Assert.IsFalse(isValid);
            Assert.AreEqual("Email must contain both '@' and '.'", results[0].ErrorMessage);
        }
    }
}
