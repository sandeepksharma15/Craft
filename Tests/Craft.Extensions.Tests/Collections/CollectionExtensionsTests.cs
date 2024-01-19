using FluentAssertions;

namespace Craft.Extensions.Tests.Collections;

public class CollectionExtensionsTests
{
    [Fact]
    public void IsNullOrEmpty_WithNullCollection_ReturnsTrue()
    {
        // Arrange
        ICollection<int>? collection = null;

        // Act
        var isEmpty = collection.IsNullOrEmpty();

        // Assert
        isEmpty.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_WithEmptyCollection_ReturnsTrue()
    {
        // Arrange
        ICollection<int> collection = [];

        // Act
        var isEmpty = collection.IsNullOrEmpty();

        // Assert
        isEmpty.Should().BeTrue();
    }

    [Fact]
    public void IsNullOrEmpty_WithNonEmptyCollection_ReturnsFalse()
    {
        // Arrange
        ICollection<int> collection = [1, 2, 3];

        // Act
        var isEmpty = collection.IsNullOrEmpty();

        // Assert
        isEmpty.Should().BeFalse();
    }
}
