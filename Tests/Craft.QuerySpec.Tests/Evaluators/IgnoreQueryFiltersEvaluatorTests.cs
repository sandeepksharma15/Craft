using Craft.QuerySpec.Core;
using Craft.QuerySpec.Evaluators;
using Craft.TestHelper.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Craft.QuerySpec.Tests.Evaluators;

public class IgnoreQueryFiltersEvaluatorTests
{
    private readonly IQueryable<Company> queryable;

    public IgnoreQueryFiltersEvaluatorTests()
    {
        queryable = new List<Company>
        {
            new() { Id = 1, Name = "Company 1" },
            new() { Id = 2, Name = "Company 2" }
        }.AsQueryable();
    }

    [Fact]
    public void GetQuery_WhenIgnoreQueryFiltersIsTrue_ReturnsIgnoreQueryFilters()
    {
        // Arrange
        var evaluator = IgnoreQueryFiltersEvaluator.Instance;
        var query = new Query<Company>();

        // Act
        query.IgnoreQueryFilters();
        var result = evaluator.GetQuery(queryable, query);

        // Assert
        result.Should().BeEquivalentTo(queryable.IgnoreQueryFilters());
        query.IgnoreQueryFilters.Should().BeTrue();
    }

    [Fact]
    public void GetQuery_WhenIgnoreQueryFiltersIsTrueForDifferentEntity_ReturnsIgnoreQueryFilters()
    {
        // Arrange
        var evaluator = IgnoreQueryFiltersEvaluator.Instance;
        var query = new Query<Company>();

        // Act
        var result = evaluator.GetQuery(queryable, query);

        // Assert
        result.Should().BeEquivalentTo(queryable);
        query.IgnoreQueryFilters.Should().BeFalse();
    }
}
