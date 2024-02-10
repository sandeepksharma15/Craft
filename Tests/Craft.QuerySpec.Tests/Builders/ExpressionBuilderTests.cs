using System.Linq.Expressions;
using Craft.QuerySpec.Builders;
using Craft.QuerySpec.Enums;
using Craft.QuerySpec.Helpers;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Builders;

public class ExpressionBuilderTests
{
    private readonly IQueryable<Company> queryable;

    public ExpressionBuilderTests()
    {
        queryable = new List<Company>
        {
            new() { Id = 1, Name = "Company 1" },
            new() { Id = 2, Name = "Company 2" }
        }.AsQueryable();
    }

    [Fact]
    public void CreateNonStringExpressionBody_ShouldReturnCorrectComparisonExpression()
    {
        // Arrange
        FilterInfo filterInfo = new(typeof(long).FullName, "Id", "1", ComparisonType.EqualTo);

        // Act
        var expression = ExpressionBuilder.CreateWhereExpression<Company>(filterInfo);
        var result = queryable.Where(expression).ToList();

        // Assert
        expression.Should().NotBeNull();
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Company 1");
    }

    [Fact]
    public void CreateWhereExpression_ShouldCreateValidExpression()
    {
        // Arrange
        Expression<Func<Company, object>> propertyExpression = (x) => x.Name;
        const string dataValue = "Company 1";
        const ComparisonType comparison = ComparisonType.EqualTo;

        // Act
        var expression = ExpressionBuilder.CreateWhereExpression(propertyExpression, dataValue, comparison);
        var result = queryable.Where(expression).ToList();

        // Assert
        expression.Should().NotBeNull();
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Company 1");
    }

    [Fact]
    public void CreateWhereExpression_ShouldReturnCorrectExpression()
    {
        // Arrange
        FilterInfo filterInfo = new(typeof(string).FullName, "Name", "Company 1", ComparisonType.EqualTo);

        // Act
        var expression = ExpressionBuilder.CreateWhereExpression<Company>(filterInfo);
        var result = queryable.Where(expression).ToList();

        // Assert
        expression.Should().NotBeNull();
        result.Should().NotBeNull();
        result.Should().HaveCount(1);
        result[0].Name.Should().Be("Company 1");
    }

    [Fact]
    public void GetPropertyExpression_ShouldReturnLambdaExpressionForValidProperty()
    {
        // Arrange
        Company instance = new() { Name = "Sample" };

        // Act
        var expression = ExpressionBuilder.GetPropertyExpression<Company>("Name");

        // Assert
        expression.Should().NotBeNull();
        var compiled = expression.Compile();
        compiled(instance).Should().Be("Sample");
    }

    [Fact]
    public void GetPropertyExpression_ShouldReturnNullForInvalidProperty()
    {
        // Act
        var expression = ExpressionBuilder.GetPropertyExpression<Company>("InvalidProperty");

        // Assert
        expression.Should().BeNull();
    }
}
