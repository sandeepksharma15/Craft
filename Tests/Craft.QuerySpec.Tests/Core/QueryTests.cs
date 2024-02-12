using Craft.QuerySpec.Core;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Core;

public class QueryTests
{
    private readonly IQueryable<Company> queryable;

    public QueryTests()
    {
        queryable = new List<Company>
        {
            new() { Id = 1, Name = "Company 1" },
            new() { Id = 2, Name = "Company 2" }
        }.AsQueryable();
    }

    [Fact]
    public void SetPage_WithValidPageSizeAndPage_ReturnsCorrectSkipAndTake()
    {
        // Arrange
        var query = new Query<Company>();
        const int pageSize = 10;
        const int page = 2;

        // Act
        query.SetPage(page, pageSize);

        // Assert
        query.Take.Should().Be(pageSize);
        query.Skip.Should().Be((page - 1) * pageSize);
    }

    [Theory]
    [InlineData(-5, 10)] // Negative page
    [InlineData(0, 10)] // Zero page
    [InlineData(1, -10)] // Negative pageSize
    [InlineData(1, 0)] // Zero pageSize
    public void SetPage_WithInvalidParameters_SetsToDefaults(int page, int pageSize)
    {
        // Arrange
        var query = new Query<Company>();

        // Act
        query.SetPage(page, pageSize);

        // Assert
        query.Take.Should().Be(10);
        query.Skip.Should().Be(10 * (1 - 1));
    }

    [Fact]
    public void IsSatisfiedBy_ReturnsTrue_ForMatchingEntity()
    {
        // Arrange
        var query = new Query<Company>().Where(e => e.Id == 1);

        // Act
        var result = query.IsSatisfiedBy(queryable.First());

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsSatisfiedBy_ReturnsFalse_ForNonMatchingEntity()
    {
        // Arrange
        var query = new Query<Company>().Where(e => e.Id == 1);

        // Act
        var result = query.IsSatisfiedBy(queryable.Last());

        // Assert
        result.Should().BeFalse();
    }
}
