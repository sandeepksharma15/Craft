using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Core;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Core;

public class QueryExtensionsTests
{
    [Fact]
    public void IgnoreQueryFilters_WhenQueryIsNull_ReturnsNull()
    {
        // Arrange
        IQuery<Company> query = null;

        // Act
        var result = query.IgnoreQueryFilters();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void IgnoreQueryFilters_WhenAsNoTrackingIsCalled_ReturnsAsNoIgnoreQueryFilters()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();

        // Act
        var result = query.IgnoreQueryFilters();

        // Assert
        result.Should().NotBeNull();
        result.IgnoreQueryFilters.Should().BeTrue();
    }

    [Fact]
    public void AsSplitQuery_WhenQueryIsNull_ReturnsNull()
    {
        // Arrange
        IQuery<Company> query = null;

        // Act
        var result = query.AsSplitQuery();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void AsSplitQuery_WhenAsNoTrackingIsCalled_ReturnsAsNoAsSplitQuery()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();

        // Act
        var result = query.AsSplitQuery();

        // Assert
        result.Should().NotBeNull();
        result.AsSplitQuery.Should().BeTrue();
    }

    [Fact]
    public void AsNoTracking_WhenQueryIsNull_ReturnsNull()
    {
        // Arrange
        IQuery<Company> query = null;

        // Act
        var result = query.AsNoTracking();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void AsNoTracking_WhenAsNoTrackingIsCalled_ReturnsAsNoTrackingQuery()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();

        // Act
        var result = query.AsNoTracking();

        // Assert
        result.Should().NotBeNull();
        result.AsNoTracking.Should().BeTrue();
    }
}
