using TaskPanelLibrary.Entity;
using TaskPanelLibrary.Exception.User;
using TaskPanelLibrary.Service;
using TaskPanelLibrary.Service.Interface;

namespace TaskPanelTest.ServiceTest;

[TestClass]
public class PasswordGeneratorServiceTest
{
    private PasswordGeneratorService _passwordGeneratorService;

    [TestInitialize]
    public void Initialize()
    {
        _passwordGeneratorService = new PasswordGeneratorService();
    }

    [TestMethod]
    public void GeneratePassword_ShouldHaveCorrectLength()
    {
        // Arrange
        var length = 12;

        // Act
        var password = _passwordGeneratorService.GeneratePassword(length);

        // Assert
        Assert.IsNotNull(password);
        Assert.AreEqual(length, password.Length);
    }

    [TestMethod]
    public void GeneratePassword_ShouldContainRequiredCharacterTypes()
    {
        // Arrange
        int length = 12;

        // Act
        var password = _passwordGeneratorService.GeneratePassword(length);

        // Assert
        Assert.IsTrue(password.Any(char.IsUpper), "Password should contain at least one uppercase letter.");
        Assert.IsTrue(password.Any(char.IsLower), "Password should contain at least one lowercase letter.");
        Assert.IsTrue(password.Any(char.IsDigit), "Password should contain at least one number.");
        Assert.IsTrue(password.Any(c => "@#$%^&*()-_=+[]{}|;:,.<>?".Contains(c)),
            "Password should contain at least one special character.");
    }

    [TestMethod]
    public void GeneratePassword_ShouldThrowException_WhenLengthIsTooSmall()
    {
        // Arrange
        int invalidLength = 3;

        // Act & Assert
        Assert.ThrowsException<ArgumentException>(() => _passwordGeneratorService.GeneratePassword(invalidLength),
            "Password length less than 4 should throw an exception.");
    }
}