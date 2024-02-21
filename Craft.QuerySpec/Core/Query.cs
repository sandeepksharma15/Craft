using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.Text.Json;
using Craft.QuerySpec.Builders;
using Craft.QuerySpec.Constants;
using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Evaluators;

namespace Craft.QuerySpec.Core;

// Represents a query with result projection.
[Serializable]
public class Query<T, TResult> : Query<T>, IQuery<T, TResult>
    where T : class
    where TResult : class
{
    // QuerySelectBuilder for constructing select expressions.
    public QuerySelectBuilder<T, TResult> QuerySelectBuilder { get; } = new();

    // Expression for selecting many results.
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany { get; set; }

    // Clears the query specifications including select expressions and selector for many results.
    public new void Clear()
    {
        base.Clear();
        QuerySelectBuilder.Clear();
        SelectorMany = null;
    }

    // Function for post-processing results.
    public new Func<IEnumerable<TResult>, IEnumerable<TResult>> PostProcessingAction { get; set; }

    public Query() { }

    public Query(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        var serializeOptions = new JsonSerializerOptions();
        serializeOptions.Converters.Add(new EntityFilterBuilderJsonConverter<T>());
        serializeOptions.Converters.Add(new SortOrderBuilderJsonConverter<T>());
        serializeOptions.Converters.Add(new SqlSearchCriteriaBuilderJsonConverter<T>());
        serializeOptions.Converters.Add(new QuerySelectBuilderJsonConverter<T, TResult>());

        info.AddValue(nameof(AsNoTracking), AsNoTracking);
        info.AddValue(nameof(AsSplitQuery), AsSplitQuery);
        info.AddValue(nameof(IgnoreAutoIncludes), IgnoreAutoIncludes);
        info.AddValue(nameof(IgnoreQueryFilters), IgnoreQueryFilters);
        info.AddValue(nameof(Skip), Skip);
        info.AddValue(nameof(Take), Take);

        info.AddValue(nameof(SortOrderBuilder), JsonSerializer.Serialize(SortOrderBuilder, serializeOptions));
        info.AddValue(nameof(SqlLikeSearchCriteriaBuilder), JsonSerializer.Serialize(SqlLikeSearchCriteriaBuilder, serializeOptions));
        info.AddValue(nameof(EntityFilterBuilder), JsonSerializer.Serialize(EntityFilterBuilder, serializeOptions));
        info.AddValue(nameof(QuerySelectBuilder), JsonSerializer.Serialize(QuerySelectBuilder, serializeOptions));
    }
}

[Serializable]
public class Query<T> : IQuery<T> where T : class
{
    // Common query specifications.
    public bool AsNoTracking { get; set; }
    public bool AsSplitQuery { get; set; }
    public bool IgnoreAutoIncludes { get; set; }
    public bool IgnoreQueryFilters { get; set; }

    // Pagination specifications.
    public int? Skip { get; set; }
    public int? Take { get; set; }

    // Builders for building where and order expressions.
    public SortOrderBuilder<T> SortOrderBuilder { get; internal set; } = new();
    public SqlLikeSearchCriteriaBuilder<T> SqlLikeSearchCriteriaBuilder { get; internal set; } = new();
    public EntityFilterBuilder<T> EntityFilterBuilder { get; internal set; } = new();

    // Function for post-processing results.
    public Func<IEnumerable<T>, IEnumerable<T>> PostProcessingAction { get; set; }

    // Checks if the entity satisfies the query specifications.
    public virtual bool IsSatisfiedBy(T entity)
    {
        // Create a queryable from the entity
        var queryable = new List<T> { entity }.AsQueryable();

        queryable = QueryEvaluator.Instance.GetQuery(queryable, this);

        return queryable.Any();
    }

    // Sets pagination specifications.
    public virtual void SetPage(int page = PaginationConstant.DefaultPage, int pageSize = PaginationConstant.DefaultPageSize)
    {
        pageSize = pageSize > 0 ? pageSize : PaginationConstant.DefaultPageSize;
        page = Math.Max(page, PaginationConstant.DefaultPage);
        Take = pageSize;
        Skip = (page - 1) * pageSize;
    }

    // Clears all query specifications.
    public void Clear()
    {
        // Reset pagination specifications.
        SetPage();

        // Reset common query specifications.
        AsNoTracking = false;
        AsSplitQuery = false;
        IgnoreAutoIncludes = false;
        IgnoreQueryFilters = false;

        // Clear Builders
        SortOrderBuilder.Clear();
        SqlLikeSearchCriteriaBuilder.Clear();
        EntityFilterBuilder.Clear();
    }

    public Query() { }

    public Query(SerializationInfo info, StreamingContext context)
    {
    }

    public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        var serializeOptions = new JsonSerializerOptions();
        serializeOptions.Converters.Add(new EntityFilterBuilderJsonConverter<T>());
        serializeOptions.Converters.Add(new SortOrderBuilderJsonConverter<T>());
        serializeOptions.Converters.Add(new SqlSearchCriteriaBuilderJsonConverter<T>());

        info.AddValue(nameof(AsNoTracking), AsNoTracking);
        info.AddValue(nameof(AsSplitQuery), AsSplitQuery);
        info.AddValue(nameof(IgnoreAutoIncludes), IgnoreAutoIncludes);
        info.AddValue(nameof(IgnoreQueryFilters), IgnoreQueryFilters);
        info.AddValue(nameof(Skip), Skip);
        info.AddValue(nameof(Take), Take);

        info.AddValue(nameof(SortOrderBuilder), JsonSerializer.Serialize(SortOrderBuilder, serializeOptions));
        info.AddValue(nameof(SqlLikeSearchCriteriaBuilder), JsonSerializer.Serialize(SqlLikeSearchCriteriaBuilder, serializeOptions));
        info.AddValue(nameof(EntityFilterBuilder), JsonSerializer.Serialize(EntityFilterBuilder, serializeOptions));
    }
}
