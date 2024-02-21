using Craft.QuerySpec.Builders;
using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Core;
using Craft.TestHelper.Models;
using FluentAssertions;
using Moq;

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

    [Fact]
    public void SetsNothing_GivenNoPostProcessingAction()
    {
        // Arrange && Act
        IQuery<Company> query = new Query<Company>();

        // Assert
        query.PostProcessingAction.Should().BeNull();
    }

    [Fact]
    public void SetPostProcessingAction_SetsAction_NotNullQuery()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();

        // Act
        var result = query.SetPostProcessingAction(x => x);

        // Assert
        result.Should().NotBeNull();
        result.PostProcessingAction.Should().NotBeNull();
    }

    [Fact]
    public void SetsNothing_TResult_GivenNoPostProcessingAction()
    {
        // Arrange && Act
        IQuery<Company> query = new Query<Company, Company>();

        // Assert
        query.PostProcessingAction.Should().BeNull();
    }

    [Fact]
    public void SetPostProcessingAction_TResult_SetsAction_NotNullQuery()
    {
        // Arrange
        IQuery<Company, Company> query = new Query<Company, Company>();

        // Act
        var result = query.SetPostProcessingAction(x => x);

        // Assert
        result.Should().NotBeNull();
        result.PostProcessingAction.Should().NotBeNull();
    }

    [Fact]
    public void IsWithoutOrder_ReturnsTrue_NoSortOrderBuilder()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();

        // Act
        var result = query.IsWithoutOrder();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsWithoutOrder_ReturnsFalse_NonEmptyOrderDescriptorList()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        query.OrderBy(x => x.Id);

        // Act
        var result = query.IsWithoutOrder();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsWithoutOrder_ReturnsTrue_NullQuery()
    {
        // Act
        var result = ((IQuery<string>)null).IsWithoutOrder();

        // Assert
        result.Should().BeTrue();
    }
}
