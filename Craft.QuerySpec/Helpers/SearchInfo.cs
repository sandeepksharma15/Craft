using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Craft.Extensions.Expressions;

namespace Craft.QuerySpec.Helpers;

// Used For an SQL LIKE Search Functionality
[Serializable]
public class SearchInfo<T> where T : class
{
    public SearchInfo(LambdaExpression searchItem, string searchTerm, int searchGroup = 1)
    {
        ArgumentNullException.ThrowIfNull(nameof(searchItem));
        ArgumentException.ThrowIfNullOrEmpty(nameof(searchTerm));

        SearchItem = searchItem;
        SearchTerm = searchTerm;
        SearchGroup = searchGroup;
    }

    internal SearchInfo()
    { }

    public int SearchGroup { get; internal set; }
    public LambdaExpression SearchItem { get; internal set; }
    public string SearchTerm { get; internal set; }
}

public class SearchInfoJsonConverter<T> : JsonConverter<SearchInfo<T>> where T : class
{
    public override SearchInfo<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
            return null;

        SearchInfo<T> searchInfo = new();

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
                break;

            if (reader.TokenType == JsonTokenType.PropertyName)
            {
                var propertyName = reader.GetString();

                reader.Read();

                if (propertyName == nameof(SearchInfo<T>.SearchItem))
                    searchInfo.SearchItem = typeof(T).CreateMemberExpression(reader.GetString());

                if (propertyName == nameof(SearchInfo<T>.SearchTerm))
                    searchInfo.SearchTerm = reader.GetString();

                if (propertyName == nameof(SearchInfo<T>.SearchGroup))
                    searchInfo.SearchGroup = reader.GetInt32();
            }
        }

        return searchInfo;
    }

    public override void Write(Utf8JsonWriter writer, SearchInfo<T> value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();

        var memberExpression = value.SearchItem.Body as MemberExpression;
        writer.WriteString(nameof(SearchInfo<T>.SearchItem), memberExpression.Member.Name);
        writer.WriteString(nameof(SearchInfo<T>.SearchTerm), value.SearchTerm);
        writer.WriteNumber(nameof(SearchInfo<T>.SearchGroup), value.SearchGroup);

        writer.WriteEndObject();
    }
}
