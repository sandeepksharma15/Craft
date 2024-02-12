using Craft.QuerySpec.Core;
using Craft.QuerySpec.Evaluators;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Evaluators;

public class OrderEvaluatorTests
{
    private readonly IQueryable<Company> queryable;

    public OrderEvaluatorTests()
    {
        queryable = new List<Company>
        {
            new() { Id = 1, Name = "Company 1" },
            new() { Id = 2, Name = "Company 2" }
        }.AsQueryable();
    }

    [Fact]
    public void GetQuery_WithNoOrderExpressions_ShouldReturnOriginalQueryable()
    {
        // Arrange
        var evaluator = OrderEvaluator.Instance;
        var query = new Query<Company>();

        // Act
        var result = evaluator.GetQuery(queryable, query);

        // Assert
        result.Should().BeSameAs(queryable);
    }

    [Fact]
    public void GetQuery_WithDuplicateOrderChain_WillAdjustSortOrders()
    {
        // Arrange
        var evaluator = OrderEvaluator.Instance;
        var query = new Query<Company>();

        query.OrderBy(x => x.Id);
        query.OrderBy(x => x.Name);

        // Act
        var result = evaluator.GetQuery(queryable, query);

        // Assert
        result.Count().Should().Be(2);
    }

    [Fact]
    public void GetQuery_WithDescendingSortOrder_WillSortInDescendingOrder()
    {
        // Arrange
        var evaluator = OrderEvaluator.Instance;
        var query = new Query<Company>();

        query.OrderByDescending(x => x.Id);

        // Act
        var result = evaluator.GetQuery(queryable, query);

        // Assert
        result.Count().Should().Be(2);
        result.First().Id.Should().Be(2);
    }
}
