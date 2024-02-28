using Craft.Utilities.Helpers;
using FluentAssertions;

namespace Craft.Utilities.Tests.Helpers;

public class PasswordGeneratorTests
{
    [Fact]
    public void GeneratePassword_ReturnsPasswordWithCorrectLength()
    {
        // Arrange
        string password = PasswordGenerator.GeneratePassword();

        // Act & Assert
        password.Length.Should().Be(8); // Check if the password length is 8 characters
    }

    [Fact]
    public void GeneratePassword_ReturnsPasswordWithDigit()
    {
        // Arrange
        string password = PasswordGenerator.GeneratePassword();

        // Act & Assert
        password.Should().MatchRegex("[0-9]"); // Check if the password contains a digit
    }

    [Fact]
    public void GeneratePassword_ReturnsPasswordWithLowercaseChar()
    {
        // Arrange
        string password = PasswordGenerator.GeneratePassword();

        // Act & Assert
        password.Should().MatchRegex("[a-z]"); // Check if the password contains a lowercase character
    }

    [Fact]
    public void GeneratePassword_ReturnsPasswordWithSpecialChar()
    {
        // Arrange
        string password = PasswordGenerator.GeneratePassword();

        // Act & Assert
        password.Should().MatchRegex(@"[!@#$%^&*()_+[\]{}|;:,.<>?]"); // Check if the password contains a special character
    }

    [Fact]
    public void GeneratePassword_ReturnsPasswordWithUppercaseChar()
    {
        // Arrange
        string password = PasswordGenerator.GeneratePassword();

        // Act & Assert
        password.Should().MatchRegex("[A-Z]"); // Check if the password contains an uppercase character
    }
}
