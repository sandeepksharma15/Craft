using Craft.QuerySpec.Core;
using Craft.QuerySpec.Evaluators;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Evaluators;

public class WhereEvaluatorTests
{
    private readonly IQueryable<Company> queryable;

    public WhereEvaluatorTests()
    {
        queryable = new List<Company>
        {
            new() { Id = 1, Name = "Company 1" },
            new() { Id = 2, Name = "Company 2" }
        }.AsQueryable();
    }

    [Fact]
    public void GetQuery_WhereExpressions_ReturnsQueryable()
    {
        // Arrange
        var evaluator = WhereEvaluator.Instance;
        var query = new Query<Company>();

        // Adds a Where expression
        query.Where(u => u.Name == "Company 1");

        // Act
        var result = evaluator.GetQuery(queryable, query).ToList();

        // Assert
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Company 1");
    }
}
