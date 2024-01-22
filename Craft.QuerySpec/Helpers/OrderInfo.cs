﻿using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Craft.Extensions.Expressions;
using Craft.QuerySpec.Enums;

namespace Craft.QuerySpec.Helpers;

[Serializable]
//[JsonConverter(typeof(OrderInfoJsonConverter<>))]
public class OrderInfo<T>(LambdaExpression orderItem, OrderTypeEnum orderType = OrderTypeEnum.OrderBy) where T : class
{
    public LambdaExpression OrderItem { get; internal set; } = orderItem;
    public OrderTypeEnum OrderType { get; internal set; } = orderType;
}

/// <summary>
/// A custom JSON converter specifically designed to serialize and deserialize instances of the OrderInfo<T> class,
/// ensuring proper handling of LambdaExpressions representing order items and OrderTypeEnum values.
/// </summary>
/// <typeparam name="T">The type of the entity being ordered.</typeparam>
public class OrderInfoJsonConverter<T> : JsonConverter<OrderInfo<T>> where T : class
{
    public override OrderInfo<T>? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        OrderInfo<T> orderInfo = new(null);

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();

                reader.Read();

                if (propertyName == nameof(OrderInfo<T>.OrderItem))
                {
                    try
                    {
                        orderInfo.OrderItem = typeof(T).CreateMemberExpression(reader.GetString());
                    }
                    catch (Exception)
                    {
                        throw new ArgumentException($"Property {propertyName} not found on type {nameof(T)}");
                    }
                }

                if (propertyName == nameof(OrderInfo<T>.OrderType))
                    orderInfo.OrderType = (OrderTypeEnum)reader.GetInt32();
            }
        }

        return orderInfo;
    }

    public override void Write(Utf8JsonWriter writer, OrderInfo<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        var memberExpression = value.OrderItem.Body as MemberExpression;
        writer.WriteString(nameof(OrderInfo<T>.OrderItem), memberExpression.Member.Name);
        writer.WriteNumber(nameof(OrderInfo<T>.OrderType), (int)value.OrderType);

        writer.WriteEndObject();
    }
}