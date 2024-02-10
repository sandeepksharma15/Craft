using System.Linq.Expressions;
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
