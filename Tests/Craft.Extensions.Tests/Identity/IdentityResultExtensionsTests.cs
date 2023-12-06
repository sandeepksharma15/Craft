using FluentAssertions;
using Microsoft.AspNetCore.Identity;

namespace Craft.Extensions.Tests.Identity;

public class IdentityResultExtensionsTests
{
    [Fact]
    public void GetErrors_WithNoErrors_ReturnsEmptyList()
    {
        // Arrange
        var result = IdentityResult.Success;

        // Act
        var errors = result.GetErrors();

        // Assert
        errors.Should().BeEmpty();
    }

    [Fact]
    public void GetErrors_WithSingleError_ReturnsListWithOneError()
    {
        // Arrange
        const string errorDescription = "Username is already taken.";
        var result = IdentityResult.Failed(new IdentityError { Description = errorDescription });

        // Act
        var errors = result.GetErrors();

        // Assert
        errors.Should().ContainSingle().And.Contain(errorDescription);
    }

    [Fact]
    public void GetErrors_WithMultipleErrors_ReturnsListWithAllErrors()
    {
        // Arrange
        var errorDescriptions = new[] { "Invalid email.", "Password must contain at least one digit." };
        var errors = errorDescriptions.Select(description => new IdentityError { Description = description }).ToArray();
        var result = IdentityResult.Failed(errors);

        // Act
        var resultErrors = result.GetErrors();

        // Assert
        resultErrors.Should().BeEquivalentTo(errorDescriptions);
    }
}
