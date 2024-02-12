using Craft.QuerySpec.Core;
using Craft.QuerySpec.Evaluators;
using Craft.TestHelper.Models;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;

namespace Craft.QuerySpec.Tests.Evaluators;

public class AsSplitQueryEvaluatorTests
{
    private readonly IQueryable<Company> queryable;

    public AsSplitQueryEvaluatorTests()
    {
        queryable = new List<Company>
        {
            new() { Id = 1, Name = "Company 1" },
            new() { Id = 2, Name = "Company 2" }
        }.AsQueryable();
    }

    [Fact]
    public void GetQuery_WhenAsSplitQueryIsTrue_ReturnsAsSplitQuery()
    {
        // Arrange
        var evaluator = AsSplitQueryEvaluator.Instance;
        var query = new Query<Company>();

        // Act
        query.AsSplitQuery();
        var result = evaluator.GetQuery(queryable, query);

        // Assert
        result.Should().BeEquivalentTo(queryable.AsSplitQuery());
        query.AsSplitQuery.Should().BeTrue();
    }

    [Fact]
    public void GetQuery_WhenAsSplitQueryIsTrueForDifferentEntity_ReturnsAsSplitQuery()
    {
        // Arrange
        var evaluator = AsSplitQueryEvaluator.Instance;
        var query = new Query<Company>();

        // Act
        var result = evaluator.GetQuery(queryable, query);

        // Assert
        result.Should().BeEquivalentTo(queryable);
        query.AsSplitQuery.Should().BeFalse();
    }
}
