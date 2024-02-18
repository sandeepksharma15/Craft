using System.Linq.Expressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using Craft.Extensions.Expressions;
using Craft.QuerySpec.Helpers;

namespace Craft.QuerySpec.Builders;

public class SearchBuilder<T> where T : class
{
    public List<SearchInfo<T>> SearchInfoList { get; }

    public SearchBuilder() => SearchInfoList = [];

    public long Count => SearchInfoList.Count;

    public SearchBuilder<T> Add(SearchInfo<T> searchInfo)
    {
        ArgumentNullException.ThrowIfNull(nameof(searchInfo));

        SearchInfoList.Add(searchInfo);
        return this;
    }

    public SearchBuilder<T> Add(Expression<Func<T, object>> member, string searchTerm, int searchGroup = 1)
    {
        SearchInfoList.Add(new SearchInfo<T>(member, searchTerm, searchGroup));
        return this;
    }

    public SearchBuilder<T> Add(string memberName, string searchTerm, int searchGroup = 1)
    {
        var member = ExpressionBuilder.GetPropertyExpression<T>(memberName);
        SearchInfoList.Add(new SearchInfo<T>(member, searchTerm, searchGroup));
        return this;
    }

    public SearchBuilder<T> Clear()
    {
        SearchInfoList.Clear();
        return this;
    }

    public SearchBuilder<T> Remove(SearchInfo<T> searchInfo)
    {
        ArgumentNullException.ThrowIfNull(nameof(searchInfo));

        SearchInfoList.Remove(searchInfo);
        return this;
    }

    public SearchBuilder<T> Remove(Expression<Func<T, object>> member)
    {
        ArgumentNullException.ThrowIfNull(nameof(member));

        var comparer = new ExpressionSemanticEqualityComparer();
        var searchInfo = SearchInfoList.Find(x => comparer.Equals(x.SearchItem, member));

        if (searchInfo != null)
            SearchInfoList.Remove(searchInfo);

        return this;
    }

    public SearchBuilder<T> Remove(string memberName)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(memberName));

        Remove(ExpressionBuilder.GetPropertyExpression<T>(memberName));

        return this;
    }
}

public class SearchBuilderJsonConverter<T> : JsonConverter<SearchBuilder<T>> where T : class
{
    private static readonly JsonSerializerOptions serializeOptions;

    static SearchBuilderJsonConverter()
    {
        serializeOptions = new JsonSerializerOptions();
        serializeOptions.Converters.Add(new SearchInfoJsonConverter<T>());
    }

    public override bool CanConvert(Type objectType)
        => objectType == typeof(SearchBuilder<T>);

    public override SearchBuilder<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }

    public override void Write(Utf8JsonWriter writer, SearchBuilder<T> value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
