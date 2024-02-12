using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Core;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Core;

public class PaginationExtensionTests
{
    [Fact]
    public void Skip_WhenQueryIsNotNull_SetsSkipCorrectly()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        const int skipValue = 10;

        // Act
        var result = query.Skip(skipValue);

        // Assert
        result.Should().NotBeNull();
        query.Skip.Should().Be(skipValue);
    }

    [Fact]
    public void Skip_WhenQueryIsNull_ReturnsNull()
    {
        // Arrange
        IQuery<Company> query = null;

        // Act
        var result = query.Skip(5);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Take_WhenQueryIsNotNull_SetsSkipCorrectly()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        const int takeValue = 10;

        // Act
        var result = query.Take(takeValue);

        // Assert
        result.Should().NotBeNull();
        query.Take.Should().Be(takeValue);
    }

    [Fact]
    public void Take_WhenQueryIsNull_ReturnsNull()
    {
        // Arrange
        IQuery<Company> query = null;

        // Act
        var result = query.Take(5);

        // Assert
        result.Should().BeNull();
    }
}
