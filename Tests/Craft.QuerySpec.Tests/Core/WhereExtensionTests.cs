using System.Linq.Expressions;
using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Core;
using Craft.QuerySpec.Enums;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace AppSpec.Tests.Core;

public class WhereExtensionTests
{
    private readonly IQueryable<Company> queryable;

    public WhereExtensionTests()
    {
        queryable = new List<Company>
        {
            new() { Id = 1, Name = "Company 1" },
            new() { Id = 2, Name = "Company 2" }
        }.AsQueryable();
    }

    [Fact]
    public void WhereWithProperyName_WhenQueryIsNotNull_AddsToWhereBuilder()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();

        // Act
        var result = query.Where("Name", "Company 2", ComparisonType.EqualTo);
        var expr = result.WhereBuilder.EntityFilterList[0];
        var filtered = queryable.Where(expr.Filter).ToList();

        // Assert
        result.Should().NotBeNull();
        filtered.Should().NotBeNull();
        filtered.Should().HaveCount(1);
        filtered[0].Name.Should().Be("Company 2");
    }

    [Fact]
    public void WhereWithProperyName_WhenPropertyNameIsNull_DoesNotAddToWhereBuilder()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        const string propName = null;
        var expected = query.WhereBuilder.EntityFilterList.Count;

        // Act
        var result = query.Where(propName, null, ComparisonType.EqualTo);

        // Assert
        result.Should().NotBeNull();
        query.WhereBuilder.EntityFilterList.Count.Should().Be(expected);
    }

    [Fact]
    public void WhereWithProperyName_WhenQueryIsNull_ReturnsNull()
    {
        // Arrange
        IQuery<Company> query = null;
        const string propName = null;

        // Act
        var result = query.Where(propName, null, ComparisonType.EqualTo);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void WhereWithPropery_WhenQueryIsNotNull_AddsToWhereBuilder()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        Expression<Func<Company, object>> propExpr = c => c.Id;

        // Act
        var result = query.Where(propExpr, 2);
        var expr = result.WhereBuilder.EntityFilterList[0];
        var filtered = queryable.Where(expr.Filter).ToList();

        // Assert
        result.Should().NotBeNull();
        filtered.Should().NotBeNull();
        filtered.Should().HaveCount(1);
        filtered[0].Name.Should().Be("Company 2");
    }

    [Fact]
    public void WhereWithPropery_WhenExpressionIsNull_DoesNotAddToWhereBuilder()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        Expression<Func<Company, object>> propExpr = null;
        var expected = query.WhereBuilder.EntityFilterList.Count;

        // Act
        var result = query.Where(propExpr, 10);

        // Assert
        result.Should().NotBeNull();
        query.WhereBuilder.EntityFilterList.Count.Should().Be(expected);
    }

    [Fact]
    public void WhereWithPropery_WhenQueryIsNull_ReturnsNull()
    {
        // Arrange
        IQuery<Company> query = null;
        Expression<Func<Company, object>> propExpr = c => c.Id;

        // Act
        var result = query.Where(propExpr, 10);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void WhereWithExpression_WhenQueryIsNotNull_AddsToWhereBuilder()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        Expression<Func<Company, bool>> expression = c => c.Id == 1;

        // Act
        var result = query.Where(expression);
        var expr = result.WhereBuilder.EntityFilterList[0];
        var filtered = queryable.Where(expr.Filter).ToList();

        // Assert
        result.Should().NotBeNull();
        filtered.Should().NotBeNull();
        filtered.Should().HaveCount(1);
        filtered[0].Name.Should().Be("Company 1");
    }

    [Fact]
    public void WhereWithExpression_WhenExpressionIsNull_DoesNotAddToWhereBuilder()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        Expression<Func<Company, bool>> expression = null;
        var expected = query.WhereBuilder.EntityFilterList.Count;

        // Act
        var result = query.Where(expression);

        // Assert
        result.Should().NotBeNull();
        query.WhereBuilder.EntityFilterList.Count.Should().Be(expected);
    }

    [Fact]
    public void WhereWithExpression_WhenQueryIsNull_ReturnsNull()
    {
        // Arrange
        IQuery<Company> query = null;
        Expression<Func<Company, bool>> expression = c => c.Id == 1;

        // Act
        var result = query.Where(expression);

        // Assert
        result.Should().BeNull();
    }
}
