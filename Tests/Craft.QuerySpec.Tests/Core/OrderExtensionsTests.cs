using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Core;
using Craft.QuerySpec.Enums;
using Craft.TestHelper.Models;
using FluentAssertions;
using System.Linq.Expressions;

namespace Craft.QuerySpec.Tests.Core;

public class OrderExtensionsTests
{
    [Fact]
    public void OrderBy_WithNullQuery_ShouldReturnNull()
    {
        // Arrange
        IQuery<Company> query = null;

        // Act
        var result = query.OrderBy(x => x.Name);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void OrderBy_WithNullExpression_ShouldReturnQuery()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        Expression<Func<Company, object>> propExpr = null;

        // Act
        var result = query.OrderBy(propExpr);

        // Assert
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void OrderBy_WithValidParameters_ShouldAddToOrderBuilder()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        Expression<Func<Company, object>> propExpr = x => x.Name;

        // Act
        var result = query.OrderBy(propExpr);

        // Assert
        result.Should().BeSameAs(query);
        query.OrderBuilder.OrderExpressions.Should().ContainSingle();
        query.OrderBuilder.OrderExpressions[0].OrderItem.Should().NotBeNull();
        query.OrderBuilder.OrderExpressions[0].OrderItem.Body.ToString().Should().Be("x.Name");
        query.OrderBuilder.OrderExpressions[0].OrderType.Should().Be(OrderTypeEnum.OrderBy);
    }

    [Fact]
    public void OrderByDescending_WithNullQuery_ShouldReturnNull()
    {
        // Arrange
        IQuery<Company> query = null;

        // Act
        var result = query.OrderByDescending(x => x.Name);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void OrderByDescending_WithNullExpression_ShouldReturnQuery()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        Expression<Func<Company, object>> propExpr = null;

        // Act
        var result = query.OrderByDescending(propExpr);

        // Assert
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void OrderByDescending_WithValidParameters_ShouldAddToOrderBuilder()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        Expression<Func<Company, object>> propExpr = x => x.Name;

        // Act
        var result = query.OrderByDescending(propExpr);

        // Assert
        result.Should().BeSameAs(query);
        query.OrderBuilder.OrderExpressions.Should().ContainSingle();
        query.OrderBuilder.OrderExpressions[0].OrderItem.Should().NotBeNull();
        query.OrderBuilder.OrderExpressions[0].OrderItem.Body.ToString().Should().Be("x.Name");
        query.OrderBuilder.OrderExpressions[0].OrderType.Should().Be(OrderTypeEnum.OrderByDescending);
    }

    [Fact]
    public void ThenBy_WithNullQuery_ShouldReturnNull()
    {
        // Arrange
        IQuery<Company> query = null;

        // Act
        var result = query.ThenBy(x => x.Name);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ThenBy_WithNullExpression_ShouldReturnQuery()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        Expression<Func<Company, object>> propExpr = null;

        // Act
        var result = query.ThenBy(propExpr);

        // Assert
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void ThenBy_WithValidParameters_ShouldAddToOrderBuilder()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        Expression<Func<Company, object>> propExpr = x => x.Name;

        // Act
        var result = query.ThenBy(propExpr);

        // Assert
        result.Should().BeSameAs(query);
        query.OrderBuilder.OrderExpressions.Should().ContainSingle();
        query.OrderBuilder.OrderExpressions[0].OrderItem.Should().NotBeNull();
        query.OrderBuilder.OrderExpressions[0].OrderItem.Body.ToString().Should().Be("x.Name");
        query.OrderBuilder.OrderExpressions[0].OrderType.Should().Be(OrderTypeEnum.ThenBy);
    }

    [Fact]
    public void ThenByDescending_WithNullQuery_ShouldReturnNull()
    {
        // Arrange
        IQuery<Company> query = null;

        // Act
        var result = query.ThenByDescending(x => x.Name);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ThenByDescending_WithNullExpression_ShouldReturnQuery()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        Expression<Func<Company, object>> propExpr = null;

        // Act
        var result = query.ThenByDescending(propExpr);

        // Assert
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void ThenByDescending_WithValidParameters_ShouldAddToOrderBuilder()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        Expression<Func<Company, object>> propExpr = x => x.Name;

        // Act
        var result = query.ThenByDescending(propExpr);

        // Assert
        result.Should().BeSameAs(query);
        query.OrderBuilder.OrderExpressions.Should().ContainSingle();
        query.OrderBuilder.OrderExpressions[0].OrderItem.Should().NotBeNull();
        query.OrderBuilder.OrderExpressions[0].OrderItem.Body.ToString().Should().Be("x.Name");
        query.OrderBuilder.OrderExpressions[0].OrderType.Should().Be(OrderTypeEnum.ThenByDescending);
    }

    [Fact]
    public void OrderBy_WithNullQuery_ShouldReturnNull_WithPropertyName()
    {
        // Arrange
        IQuery<Company> query = null;

        // Act
        var result = query.OrderBy("Name");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void OrderBy_WithNullPropertyName_ShouldReturnQuery()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        const string propName = null;

        // Act
        var result = query.OrderBy(propName);

        // Assert
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void OrderBy_WithValidParameters_ShouldAddToOrderBuilder_WithPropertyName()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        const string propName = "Name";

        // Act
        var result = query.OrderBy(propName);

        // Assert
        result.Should().BeSameAs(query);
        query.OrderBuilder.OrderExpressions.Should().ContainSingle();
        query.OrderBuilder.OrderExpressions[0].OrderItem.Should().NotBeNull();
        query.OrderBuilder.OrderExpressions[0].OrderType.Should().Be(OrderTypeEnum.OrderBy);
    }

    [Fact]
    public void OrderByDescending_WithNullQuery_ShouldReturnNull_WithPropertyName()
    {
        // Arrange
        IQuery<Company> query = null;

        // Act
        var result = query.OrderByDescending("Name");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void OrderByDescending_WithNullPropertyName_ShouldReturnQuery()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        const string propName = null;

        // Act
        var result = query.OrderByDescending(propName);

        // Assert
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void OrderByDescending_WithValidParameters_ShouldAddToOrderBuilder_WithPropertyName()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        const string propName = "Name";

        // Act
        var result = query.OrderByDescending(propName);

        // Assert
        result.Should().BeSameAs(query);
        query.OrderBuilder.OrderExpressions.Should().ContainSingle();
        query.OrderBuilder.OrderExpressions[0].OrderItem.Should().NotBeNull();
        query.OrderBuilder.OrderExpressions[0].OrderType.Should().Be(OrderTypeEnum.OrderByDescending);
    }

    [Fact]
    public void ThenBy_WithNullQuery_ShouldReturnNull_WithPropertyName()
    {
        // Arrange
        IQuery<Company> query = null;

        // Act
        var result = query.ThenBy("Name");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ThenBy_WithNullPropertyName_ShouldReturnQuery()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        const string propName = null;

        // Act
        var result = query.ThenBy(propName);

        // Assert
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void ThenBy_WithValidParameters_ShouldAddToOrderBuilder_WithPropertyName()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        const string propName = "Name";

        // Act
        var result = query.ThenBy(propName);

        // Assert
        result.Should().BeSameAs(query);
        query.OrderBuilder.OrderExpressions.Should().ContainSingle();
        query.OrderBuilder.OrderExpressions[0].OrderItem.Should().NotBeNull();
        query.OrderBuilder.OrderExpressions[0].OrderType.Should().Be(OrderTypeEnum.ThenBy);
    }

    [Fact]
    public void ThenByDescending_WithNullQuery_ShouldReturnNull_WithPropertyName()
    {
        // Arrange
        IQuery<Company> query = null;

        // Act
        var result = query.ThenByDescending("Name");

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ThenByDescending_WithNullPropertyName_ShouldReturnQuery()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        const string propName = null;

        // Act
        var result = query.ThenByDescending(propName);

        // Assert
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void ThenByDescending_WithValidParameters_ShouldAddToOrderBuilder_WithPropertyName()
    {
        // Arrange
        IQuery<Company> query = new Query<Company>();
        const string propName = "Name";

        // Act
        var result = query.ThenByDescending(propName);

        // Assert
        result.Should().BeSameAs(query);
        query.OrderBuilder.OrderExpressions.Should().ContainSingle();
        query.OrderBuilder.OrderExpressions[0].OrderItem.Should().NotBeNull();
        query.OrderBuilder.OrderExpressions[0].OrderType.Should().Be(OrderTypeEnum.ThenByDescending);
    }
}
