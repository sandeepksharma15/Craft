using System.Linq.Expressions;
using Craft.QuerySpec.Builders;
using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Evaluators;

namespace Craft.QuerySpec.Core;

[Serializable]
public class Query<T, TResult> : Query<T>, IQuery<T, TResult>
    where T : class
    where TResult : class
{
    public SelectBuilder<T, TResult> SelectBuilder { get; } = new();
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany { get; set; }

    public new void Clear()
    {
        base.Clear();
        SelectBuilder.Clear();
        SelectorMany = null;
    }

    public new Func<IEnumerable<TResult>, IEnumerable<TResult>> PostProcessingAction { get; set; }

    public Query()
    { }
}

[Serializable]
public class Query<T> : IQuery<T> where T : class
{
    const int DefaultPage = 1;
    const int DefaultPageSize = 10;

    public bool AsNoTracking { get; set; }
    public bool AsSplitQuery { get; set; }
    public bool IgnoreAutoIncludes { get; set; }
    public bool IgnoreQueryFilters { get; set; }

    public int? Skip { get; set; }
    public int? Take { get; set; }

    public OrderBuilder<T> OrderBuilder { get; internal set; } = new();
    public WhereBuilder<T> WhereBuilder { get; internal set; } = new();

    public Func<IEnumerable<T>, IEnumerable<T>> PostProcessingAction { get; set; }

    public virtual bool IsSatisfiedBy(T entity)
    {
        // Create a queryable from the entity
        var queryable = new List<T> { entity }.AsQueryable();

        queryable = QueryEvaluator.Instance.GetQuery(queryable, this);

        return queryable.Any();
    }

    public virtual void SetPage(int page = DefaultPage, int pageSize = DefaultPageSize)
    {
        pageSize = pageSize > 0 ? pageSize : DefaultPageSize;
        page = page > 0 ? page : DefaultPage;
        Take = pageSize;
        Skip = (page - 1) * pageSize;
    }

    public void Clear()
    {
        SetPage();
        AsNoTracking = false;
        AsSplitQuery = false;
        IgnoreAutoIncludes = false;
        IgnoreQueryFilters = false;
        WhereBuilder.Clear();
        OrderBuilder.Clear();
    }
}
