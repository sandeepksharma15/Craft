using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Craft.Extensions.System;
using Craft.QuerySpec.Builders;

namespace Craft.QuerySpec.Helpers;

/// <summary>
/// Represents a specification for filtering a collection of entities of type `T`.
/// Encapsulates a compiled predicate function for efficient evaluation.
/// </summary>
/// <typeparam name="T">The type of the entities to be filtered.</typeparam>
[Serializable]
public class WhereInfo<T> where T : class
{
    #region Private Fields

    private readonly Lazy<Func<T, bool>> filterFunc;

    #endregion Private Fields

    #region Public Properties

    public Expression<Func<T, bool>> Filter { get; }
    public Func<T, bool> FilterFunc => filterFunc.Value;

    #endregion Public Properties

    #region Public Constructors

    public WhereInfo(Expression<Func<T, bool>> filter)
    {
        ArgumentNullException.ThrowIfNull(filter);

        Filter = filter;
        filterFunc = new Lazy<Func<T, bool>>(Filter.Compile);
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>
    /// Checks whether the specified entity matches the filter criteria.
    /// </summary>
    /// <param name="entity">The entity to be evaluated.</param>
    /// <returns>True if the entity matches the filter, false otherwise.</returns>
    public bool Matches(T entity) => FilterFunc.Invoke(entity);

    #endregion Public Methods
}

/// <summary>
/// A custom JSON converter for the `WhereInfo<T>` class, enabling serialization and deserialization of filter expressions for entity filtering.
/// </summary>
/// <typeparam name="T">The type of the entities being filtered.</typeparam>
public class WhereInfoJsonConverter<T> : JsonConverter<WhereInfo<T>> where T : class
{
    #region Public Methods

    public override WhereInfo<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        Expression<Func<T, bool>> filter = null;

        if (reader.TokenType == JsonTokenType.Null)
            return null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();

                reader.Read();

                if (propertyName == nameof(WhereInfo<T>.Filter))
                {
                    var str = reader.GetString();

                    str = str.Replace('\'', '\"');
                    str = str.RemovePreFix("(");
                    str = str.RemovePostFix(")");

                    filter = ExpressionTreeBuilder.BuildBinaryTreeExpression<T>(str);
                }
                else
                {
                    throw new JsonException($"Expected a valid property value instead of {reader.GetString()}");
                }
            }
        }

        return new WhereInfo<T>(filter);
    }

    public override void Write(Utf8JsonWriter writer, WhereInfo<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        writer.WriteString(nameof(WhereInfo<T>.Filter), value.Filter.Body.ToString());

        writer.WriteEndObject();
    }

    #endregion Public Methods
}
