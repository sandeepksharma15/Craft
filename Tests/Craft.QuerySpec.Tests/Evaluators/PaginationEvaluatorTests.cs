using Craft.QuerySpec.Core;
using Craft.QuerySpec.Evaluators;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Evaluators;

public class PaginationEvaluatorTests
{
    private readonly IQueryable<Company> queryable;

    public PaginationEvaluatorTests()
    {
        queryable = new List<Company>
        {
            new() { Id = 1, Name = "Company 1" },
            new() { Id = 2, Name = "Company 2" }
        }.AsQueryable();
    }

    [Fact]
    public void GetQuery_WithSkip_ReturnsQueryableWithSkip()
    {
        // Arrange
        var evaluator = PaginationEvaluator.Instance;
        var query = new Query<Company>();
        query.Skip(1);
        query.Take(null);

        // Act
        var result = evaluator.GetQuery(queryable, query);

        // Assert
        result.Expression.ToString().Should().Contain("Skip(1)");
        result.Expression.ToString().Should().NotContain("Take");
    }

    [Fact]
    public void GetQuery_WithTake_ReturnsQueryableWithTake()
    {
        // Arrange
        var evaluator = PaginationEvaluator.Instance;
        var query = new Query<Company>();
        query.Skip(null);
        query.Take(2);

        // Act
        var result = evaluator.GetQuery(queryable, query);

        // Assert
        result.Expression.ToString().Should().NotContain("Skip");
        result.Expression.ToString().Should().Contain("Take(2)");
    }

    [Fact]
    public void GetQuery_WithSkipAndTake_ReturnsQueryableWithSkipAndTake()
    {
        // Arrange
        var evaluator = PaginationEvaluator.Instance;
        var query = new Query<Company>();
        query.Skip(1);
        query.Take(2);

        // Act
        var result = evaluator.GetQuery(queryable, query);

        // Assert
        result.Expression.ToString().Should().Contain("Skip(1)");
        result.Expression.ToString().Should().Contain("Take(2)");
    }

    [Fact]
    public void GetQuery_WithSkipAndTake_ReturnsQueryableWithSkipAndTakeData()
    {
        // Arrange
        var evaluator = PaginationEvaluator.Instance;
        var query = new Query<Company>();
        query.Skip(1);
        query.Take(2);

        // Act
        var result = evaluator.GetQuery(queryable, query).ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Company 2");
    }
}
