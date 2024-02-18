using System.Linq.Expressions;
using System.Text.Json;
using Craft.QuerySpec.Builders;
using Craft.QuerySpec.Enums;
using Craft.QuerySpec.Helpers;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Builders;

public class OrderBuilderTests
{
    private readonly JsonSerializerOptions serializeOptions;

    public OrderBuilderTests()
    {
        // Arrange
        serializeOptions = new JsonSerializerOptions();
        serializeOptions.Converters.Add(new OrderBuilderJsonConverter<Company>());
    }

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

    [Fact]
    public void OrderBuilder_Should_Initialize_OrderExpressions_List()
    {
        // Arrange & Act
        var orderBuilder = new OrderBuilder<Company>();

        // Assert
        orderBuilder.OrderExpressions.Should().NotBeNull();
        orderBuilder.OrderExpressions.Should().BeEmpty();
    }

    [Fact]
    public void OrderBuilder_Add_Should_Add_OrderInfo_To_OrderExpressions()
    {
        // Arrange
        var orderBuilder = new OrderBuilder<Company>();
        Expression<Func<Company, object>> orderExpr = x => x.Id;
        var orderInfo = new OrderInfo<Company>(orderExpr);

        // Act
        orderBuilder.Add(orderInfo);

        // Assert
        orderBuilder.OrderExpressions.Should().Contain(orderInfo);
    }

    [Fact]
    public void Write_Should_Serialize_OrderBuilder_OrderExpressions()
    {
        // Arrange
        var orderBuilder = new OrderBuilder<Company>();
        orderBuilder.Add(c => c.Name);
        orderBuilder.Add(c => c.Id);

        // Act
        var serializedOrderBuilder = JsonSerializer.Serialize(orderBuilder, serializeOptions);

        // Assert
        serializedOrderBuilder.Should().Contain("Name");
        serializedOrderBuilder.Should().Contain("Id");
    }

    [Fact]
    public void Read_Should_Deserialize_OrderBuilder_With_OrderExpressions()
    {
        // Arrange
        const string json = "[{\"OrderItem\":\"Name\",\"OrderType\":1},{\"OrderItem\":\"Id\",\"OrderType\":3}]";

        // Act
        var orderBuilder = JsonSerializer.Deserialize<OrderBuilder<Company>>(json, serializeOptions);

        // Assert
        orderBuilder.Should().NotBeNull();
        orderBuilder.OrderExpressions.Should().HaveCount(2);
        orderBuilder.OrderExpressions[0].OrderItem.Body.ToString().Should().Contain("x.Name");
        orderBuilder.OrderExpressions[1].OrderItem.Body.ToString().Should().Contain("x.Id");
    }
}
