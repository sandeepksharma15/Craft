using System.Text.Json;
using Craft.QuerySpec.Enums;

namespace Conceptualize;

internal static class SerializeList
{
    // Create a list of orders.
    public static List<Order> CreateOrders()
    {
        return [
            new Order { OrderItem = "Item 1", OrderType = OrderTypeEnum.OrderBy },
            new Order { OrderItem = "Item 2", OrderType = OrderTypeEnum.OrderBy },
            new Order { OrderItem = "Item 3", OrderType = OrderTypeEnum.OrderByDescending }
        ];
    }

    // Serialize a List of orders to a JSON string.
    public static string SerializeOrders(List<Order> orders)
        => JsonSerializer.Serialize(orders);
}

internal class Order
{
    public string OrderItem { get; set; }
    public OrderTypeEnum OrderType { get; set; }
}
