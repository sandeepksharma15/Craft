using System.Linq.Expressions;
using System.Text.Json;
using Craft.QuerySpec.Enums;
using Craft.QuerySpec.Helpers;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Helpers;

public class OrderInfoTests
{
    private readonly JsonSerializerOptions serializeOptions;

    public OrderInfoTests()
    {
        serializeOptions = new JsonSerializerOptions();
        serializeOptions.Converters.Add(new OrderInfoJsonConverter<MyTestClass>());
    }

    [Fact]
    public void Constructor_WithValidOrderItem_InitializesProperties()
    {
        // Arrange
        Expression<Func<string, object?>> orderItemExpression = s => s.Length;
        var orderInfo = new OrderInfo<string>(orderItemExpression, OrderTypeEnum.OrderBy);

        // Act
        var orderType = orderInfo.OrderType;

        // Assert
        orderInfo.OrderItem.Should().BeEquivalentTo(orderItemExpression);
        orderType.Should().Be(OrderTypeEnum.OrderBy);
    }

    [Fact]
    public void Read_WithInvalidMemberExpression_ThrowsException()
    {
        // Arrange
        const string json = @"{""OrderItem"": ""InvalidMember"", ""OrderType"": 0}";

        // Act
        var act = () => JsonSerializer.Deserialize<OrderInfo<MyTestClass>>(json, serializeOptions);

        // Assert
        act.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Read_WithNullJson_ReturnsNull()
    {
        // Arrange
        const string json = "null";

        // Act
        var orderInfo = JsonSerializer.Deserialize<OrderInfo<MyTestClass>>(json, serializeOptions);

        // Assert
        orderInfo.Should().BeNull();
    }

    [Fact]
    public void Serialization_RoundTrip_ReturnsEqualOrderInfo()
    {
        // Arrange
        Expression<Func<MyTestClass, object>> orderItemExpression = x => x.MyProperty;
        var orderInfo = new OrderInfo<MyTestClass>(orderItemExpression, OrderTypeEnum.OrderByDescending);

        // Act
        var serializationInfo = JsonSerializer.Serialize(orderInfo, serializeOptions);
        var deserializedOrderInfo = JsonSerializer.Deserialize<OrderInfo<MyTestClass>>(serializationInfo, serializeOptions);

        // Assert
        deserializedOrderInfo.OrderType.Should().Be(OrderTypeEnum.OrderByDescending);

        var compiledDelegate = deserializedOrderInfo.OrderItem.Compile();
        var myTestClass = new MyTestClass { MyProperty = "TestValue" };
        var propertyValue = compiledDelegate.DynamicInvoke(myTestClass);
        propertyValue.Should().Be("TestValue");
    }
}

public class MyTestClass
{
    public int Id { get; set; }
    public string MyProperty { get; set; }
}
