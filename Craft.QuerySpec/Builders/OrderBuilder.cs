using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Craft.Extensions.Expressions;
using Craft.QuerySpec.Enums;
using Craft.QuerySpec.Helpers;

namespace Craft.QuerySpec.Builders;

/// <summary>
/// Builder class for creating order expressions.
/// </summary>
[Serializable]
public class OrderBuilder<T> where T : class
{
    /// <summary>
    /// Constructor to initialize the OrderBuilder.
    /// </summary>
    public OrderBuilder() => OrderExpressions = [];

    /// <summary>
    /// List of order expressions.
    /// </summary>
    public List<OrderInfo<T>> OrderExpressions { get; }

    public long Count => OrderExpressions.Count;

    public OrderBuilder<T> Add(OrderInfo<T> orderInfo)
    {
        ArgumentNullException.ThrowIfNull(nameof(orderInfo));
        orderInfo.OrderType = AdjustOrderType(orderInfo.OrderType);
        OrderExpressions.Add(orderInfo);
        return this;
    }

    /// <summary>
    /// Adds an order expression based on a property expression.
    /// </summary>
    public OrderBuilder<T> Add(Expression<Func<T, object>> propExpr, OrderTypeEnum orderType = OrderTypeEnum.OrderBy)
    {
        ArgumentNullException.ThrowIfNull(nameof(propExpr));
        OrderExpressions.Add(new OrderInfo<T>(propExpr, AdjustOrderType(orderType)));
        return this;
    }

    /// <summary>
    /// Adds an order expression based on a property name.
    /// </summary>
    public OrderBuilder<T> Add(string propName, OrderTypeEnum orderType = OrderTypeEnum.OrderBy)
    {
        var propExpr = ExpressionBuilder.GetPropertyExpression<T>(propName);
        OrderExpressions.Add(new OrderInfo<T>(propExpr, AdjustOrderType(orderType)));
        return this;
    }

    /// <summary>
    /// Clears all order expressions.
    /// </summary>
    public OrderBuilder<T> Clear()
    {
        OrderExpressions.Clear();
        return this;
    }

    /// <summary>
    /// Removes an order expression based on a property expression.
    /// </summary>
    public OrderBuilder<T> Remove(Expression<Func<T, object>> propExpr)
    {
        ArgumentNullException.ThrowIfNull(nameof(propExpr));
        var comparer = new ExpressionSemanticEqualityComparer();
        var orderInfo = OrderExpressions.Find(x => comparer.Equals(x.OrderItem, propExpr));

        if (orderInfo != null)
            OrderExpressions.Remove(orderInfo);

        return this;
    }

    /// <summary>
    /// Removes an order expression based on a property name.
    /// </summary>
    public OrderBuilder<T> Remove(string propName)
    {
        Remove(ExpressionBuilder.GetPropertyExpression<T>(propName));
        return this;
    }

    /// <summary>
    /// Adjusts the order type based on existing expressions.
    /// </summary>
    internal OrderTypeEnum AdjustOrderType(OrderTypeEnum orderType)
    {
        if (OrderExpressions.Any(x => x.OrderType is OrderTypeEnum.OrderBy or OrderTypeEnum.OrderByDescending))
            if (orderType is OrderTypeEnum.OrderBy)
                orderType = OrderTypeEnum.ThenBy;
            else if (orderType is OrderTypeEnum.OrderByDescending)
                orderType = OrderTypeEnum.ThenByDescending;
        return orderType;
    }
}

public class OrderBuilderJsonConverter<T> : JsonConverter<OrderBuilder<T>> where T : class
{
    private static readonly JsonSerializerOptions serializeOptions;

    static OrderBuilderJsonConverter()
    {
        serializeOptions = new JsonSerializerOptions();
        serializeOptions.Converters.Add(new OrderInfoJsonConverter<T>());
    }

    public override bool CanConvert(Type objectType)
        => objectType == typeof(OrderBuilder<T>);

    public override OrderBuilder<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Create a new OrderBuilder
        var orderBuilder = new OrderBuilder<T>();

        // We Want To Clone The Options To Add The OrderInfoJsonConverter
        var localOptions = options.GetClone();
        localOptions.Converters.Add(new OrderInfoJsonConverter<T>());

        // Check for array start
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Invalid format for OrderBuilder: expected array of OrderInfo");

        // Read each order expression
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;

            // Read the individual order expression object
            var orderInfo = JsonSerializer.Deserialize<OrderInfo<T>>(ref reader, localOptions);

            // Validate and add the order expression
            if (orderInfo != null)
                orderBuilder.Add(orderInfo);
            else
                throw new JsonException("Invalid order expression encountered in OrderBuilder array.");
        }

        return orderBuilder;
    }

    public override void Write(Utf8JsonWriter writer, OrderBuilder<T> value, JsonSerializerOptions options)
    {
        // Start The Array
        writer.WriteStartArray();

        foreach (var order in value.OrderExpressions)
        {
            var json = JsonSerializer.Serialize(order, serializeOptions);
            writer.WriteRawValue(json);
        }

        // End the array
        writer.WriteEndArray();
    }
}
