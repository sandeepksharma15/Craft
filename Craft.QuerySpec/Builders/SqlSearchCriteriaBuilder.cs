﻿using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Craft.Extensions.Expressions;
using Craft.QuerySpec.Helpers;

namespace Craft.QuerySpec.Builders;

public class SqlSearchCriteriaBuilder<T> where T : class
{
    public List<SqlLikeSearchInfo<T>> SearchCriteriaList { get; }

    public SqlSearchCriteriaBuilder() => SearchCriteriaList = [];

    public long Count => SearchCriteriaList.Count;

    public SqlSearchCriteriaBuilder<T> Add(SqlLikeSearchInfo<T> searchInfo)
    {
        ArgumentNullException.ThrowIfNull(nameof(searchInfo));

        SearchCriteriaList.Add(searchInfo);
        return this;
    }

    public SqlSearchCriteriaBuilder<T> Add(Expression<Func<T, object>> member, string searchString, int searchGroup = 1)
    {
        SearchCriteriaList.Add(new SqlLikeSearchInfo<T>(member, searchString, searchGroup));
        return this;
    }

    public SqlSearchCriteriaBuilder<T> Add(string memberName, string searchString, int searchGroup = 1)
    {
        var member = ExpressionBuilder.GetPropertyExpression<T>(memberName);
        SearchCriteriaList.Add(new SqlLikeSearchInfo<T>(member, searchString, searchGroup));
        return this;
    }

    public SqlSearchCriteriaBuilder<T> Clear()
    {
        SearchCriteriaList.Clear();
        return this;
    }

    public SqlSearchCriteriaBuilder<T> Remove(SqlLikeSearchInfo<T> searchInfo)
    {
        ArgumentNullException.ThrowIfNull(nameof(searchInfo));

        SearchCriteriaList.Remove(searchInfo);
        return this;
    }

    public SqlSearchCriteriaBuilder<T> Remove(Expression<Func<T, object>> member)
    {
        ArgumentNullException.ThrowIfNull(nameof(member));

        var comparer = new ExpressionSemanticEqualityComparer();
        var searchInfo = SearchCriteriaList.Find(x => comparer.Equals(x.SearchItem, member));

        if (searchInfo != null)
            SearchCriteriaList.Remove(searchInfo);

        return this;
    }

    public SqlSearchCriteriaBuilder<T> Remove(string memberName)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(memberName));

        Remove(ExpressionBuilder.GetPropertyExpression<T>(memberName));

        return this;
    }
}

public class SearchBuilderJsonConverter<T> : JsonConverter<SqlSearchCriteriaBuilder<T>> where T : class
{
    public override bool CanConvert(Type objectType)
        => objectType == typeof(SqlSearchCriteriaBuilder<T>);

    public override SqlSearchCriteriaBuilder<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var searchBuilder = new SqlSearchCriteriaBuilder<T>();

        // We Want To Clone The Options To Add The SqlLikeSearchInfoJsonConverter
        var localOptions = options.GetClone();
        localOptions.Converters.Add(new SqlLikeSearchInfoJsonConverter<T>());

        // Check for array start
        if (reader.TokenType != JsonTokenType.StartArray)
            throw new JsonException("Invalid format for SqlSearchCriteriaBuilder: expected array of SqlLikeSearchInfo");

        // Read each order expression
        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndArray)
                break;

            // Read the individual SqlLikeSearchInfo object
            var searchInfo = JsonSerializer.Deserialize<SqlLikeSearchInfo<T>>(ref reader, localOptions);

            // Validate and add the order expression
            if (searchInfo != null)
                searchBuilder.Add(searchInfo);
            else
                throw new JsonException("Invalid SqlLikeSearchInfo encountered in SqlSearchCriteriaBuilder array");
        }

        return searchBuilder;
    }

    public override void Write(Utf8JsonWriter writer, SqlSearchCriteriaBuilder<T> value, JsonSerializerOptions options)
    {
        // Start The Array
        writer.WriteStartArray();

        // We Want To Clone The Options To Add The SqlLikeSearchInfoJsonConverter
        var localOptions = options.GetClone();
        localOptions.Converters.Add(new SqlLikeSearchInfoJsonConverter<T>());

        foreach (var searchInfo in value.SearchCriteriaList)
        {
            var json = JsonSerializer.Serialize(searchInfo, localOptions);
            writer.WriteRawValue(json);
        }

        // End the array
        writer.WriteEndArray();
    }
}
