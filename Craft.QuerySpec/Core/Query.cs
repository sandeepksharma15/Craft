using System.Linq.Expressions;
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
    // SelectBuilder for constructing select expressions.
    public SelectBuilder<T, TResult> SelectBuilder { get; } = new();

    // Expression for selecting many results.
    public Expression<Func<T, IEnumerable<TResult>>>? SelectorMany { get; set; }

    // Clears the query specifications including select expressions and selector for many results.
    public new void Clear()
    {
        base.Clear();
        SelectBuilder.Clear();
        SelectorMany = null;
    }

    // Function for post-processing results.
    public new Func<IEnumerable<TResult>, IEnumerable<TResult>> PostProcessingAction { get; set; }
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
    public OrderBuilder<T> OrderBuilder { get; internal set; } = new();
    public SearchBuilder<T> SearchBuilder { get; internal set; } = new();
    public WhereBuilder<T> WhereBuilder { get; internal set; } = new();

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
        OrderBuilder.Clear();
        SearchBuilder.Clear();
        WhereBuilder.Clear();
    }
}
