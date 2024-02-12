using Craft.QuerySpec.Core;
using Craft.QuerySpec.Evaluators;
using Craft.TestHelper.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Craft.QuerySpec.Tests.Evaluators;

public class AsNoTrackingEvaluatorTests
{
    private readonly IQueryable<Company> queryable;

    public AsNoTrackingEvaluatorTests()
    {
        queryable = new List<Company>
        {
            new() { Id = 1, Name = "Company 1" },
            new() { Id = 2, Name = "Company 2" }
        }.AsQueryable();
    }

    [Fact]
    public void GetQuery_WhenAsNoTrackingIsTrue_ReturnsAsNoTrackingQuery()
    {
        // Arrange
        var evaluator = AsNoTrackingEvaluator.Instance;
        var query = new Query<Company>();

        // Act
        query.AsNoTracking();
        var result = evaluator.GetQuery(queryable, query);

        // Assert
        result.Should().BeEquivalentTo(queryable.AsNoTracking());
        query.AsNoTracking.Should().BeTrue();
    }

    [Fact]
    public void GetQuery_WhenAsNoTrackingIsTrueForDifferentEntity_ReturnsAsNoTrackingQuery()
    {
        // Arrange
        var evaluator = AsNoTrackingEvaluator.Instance;
        var query = new Query<Company>();

        // Act
        var result = evaluator.GetQuery(queryable, query);

        // Assert
        result.Should().BeEquivalentTo(queryable);
        query.AsNoTracking.Should().BeFalse();
    }
}
