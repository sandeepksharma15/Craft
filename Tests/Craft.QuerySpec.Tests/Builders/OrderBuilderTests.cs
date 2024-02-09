using System.Linq.Expressions;
using Craft.QuerySpec.Builders;
using Craft.QuerySpec.Enums;
using Craft.QuerySpec.Helpers;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Builders;

public class OrderBuilderTests
{
    #region Public Methods

    [Fact]
    public void Add_Method_Should_Add_OrderExpression()
    {
        // Arrange
        var orderBuilder = new OrderBuilder<Company>();

        // Act
        orderBuilder.Add(x => x.Name);

        // Assert
        orderBuilder.OrderExpressions.Should().NotBeEmpty();
        orderBuilder.OrderExpressions[0].OrderItem.Should().NotBeNull();
        orderBuilder.OrderExpressions[0].OrderItem.Body.ToString().Should().Be("x.Name");
    }

    [Fact]
    public void Add_WithExistingOrderBy_ShouldAdjustOrderTypeToThenBy()
    {
        // Arrange
        var orderBuilder = new OrderBuilder<Company>();
        Expression<Func<Company, object>> propExpr1 = x => x.Id;
        Expression<Func<Company, object>> propExpr2 = x => x.Name;
        orderBuilder.Add(propExpr1);

        // Act
        var result = orderBuilder.Add(propExpr2, OrderTypeEnum.OrderBy);

        // Assert
        result.OrderExpressions[1].OrderType.Should().Be(OrderTypeEnum.ThenBy);
    }

    [Fact]
    public void Add_WithExistingOrderByDescending_ShouldAdjustOrderTypeToThenByDescending()
    {
        // Arrange
        var orderBuilder = new OrderBuilder<Company>();
        Expression<Func<Company, object>> propExpr1 = x => x.Id;
        Expression<Func<Company, object>> propExpr2 = x => x.Name;
        orderBuilder.Add(propExpr1, OrderTypeEnum.OrderByDescending);

        // Act
        var result = orderBuilder.Add(propExpr2, OrderTypeEnum.OrderBy);

        // Assert
        result.OrderExpressions[1].OrderType.Should().Be(OrderTypeEnum.ThenBy);
    }

    [Fact]
    public void AddProperty_Method_Should_Add_OrderExpression()
    {
        // Arrange
        var orderBuilder = new OrderBuilder<Company>();

        // Act
        orderBuilder.Add("Name");

        // Assert
        orderBuilder.OrderExpressions.Should().NotBeEmpty();
        orderBuilder.OrderExpressions[0].OrderItem.Should().NotBeNull();
    }

    [Theory]
    [InlineData(OrderTypeEnum.OrderBy)]
    [InlineData(OrderTypeEnum.OrderByDescending)]
    public void AdjustOrderType_WhenExistingOrderExpressionsPresent_ShouldReturnThenBy(OrderTypeEnum existingOrderType)
    {
        // Arrange
        var orderBuilder = new OrderBuilder<object>();
        orderBuilder.OrderExpressions.Add(new OrderInfo<object>(null, existingOrderType));

        // Act
        var adjustedOrderType = orderBuilder.AdjustOrderType(OrderTypeEnum.OrderBy);

        // Assert
        adjustedOrderType.Should().Be(OrderTypeEnum.ThenBy);
    }

    [Theory]
    [InlineData(OrderTypeEnum.OrderBy)]
    [InlineData(OrderTypeEnum.OrderByDescending)]
    public void AdjustOrderType_WhenExistingOrderExpressionsPresentAndOrderTypeIsDescending_ShouldReturnThenByDescending(OrderTypeEnum existingOrderType)
    {
        // Arrange
        var orderBuilder = new OrderBuilder<object>();
        orderBuilder.OrderExpressions.Add(new OrderInfo<object>(null, existingOrderType));

        // Act
        var adjustedOrderType = orderBuilder.AdjustOrderType(OrderTypeEnum.OrderByDescending);

        // Assert
        adjustedOrderType.Should().Be(OrderTypeEnum.ThenByDescending);
    }

    [Fact]
    public void AdjustOrderType_WhenNoExistingOrderExpressions_ShouldReturnOriginalOrderType()
    {
        // Arrange
        var orderBuilder = new OrderBuilder<object>();

        // Act
        var adjustedOrderType = orderBuilder.AdjustOrderType(OrderTypeEnum.OrderBy);

        // Assert
        adjustedOrderType.Should().Be(OrderTypeEnum.OrderBy);
    }

    [Fact]
    public void Clear_Method_Should_Empty_OrderExpressions()
    {
        // Arrange
        var orderBuilder = new OrderBuilder<Company>();
        orderBuilder.Add(x => x.Name);

        // Act
        orderBuilder.Clear();

        // Assert
        orderBuilder.OrderExpressions.Should().BeEmpty();
    }

    [Fact]
    public void Remove_Method_Should_Remove_OrderExpression()
    {
        // Arrange
        var orderBuilder = new OrderBuilder<Company>();
        Expression<Func<Company, object>> orderExpr = x => x.Name;
        orderBuilder.Add(orderExpr);

        // Act
        orderBuilder.Remove(orderExpr);

        // Assert
        orderBuilder.OrderExpressions.Should().BeEmpty();
    }

    [Fact]
    public void RemoveProperty_Method_Should_Remove_OrderExpression()
    {
        // Arrange
        var orderBuilder = new OrderBuilder<Company>();
        orderBuilder.Add("Name");

        // Act
        orderBuilder.Remove("Name");

        // Assert
        orderBuilder.OrderExpressions.Should().BeEmpty();
    }

    #endregion Public Methods
}
