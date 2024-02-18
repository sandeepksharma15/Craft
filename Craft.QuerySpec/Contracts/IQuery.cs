using System.Linq.Expressions;
using Craft.QuerySpec.Builders;

namespace Craft.QuerySpec.Contracts;

public interface IQuery<T, TResult> : IQuery<T>
    where T : class
    where TResult : class
{
    new Func<IEnumerable<TResult>, IEnumerable<TResult>> PostProcessingAction { get; internal set; }

    SelectBuilder<T, TResult> SelectBuilder { get; }

    Expression<Func<T, IEnumerable<TResult>>> SelectorMany { get; internal set; }

    new void Clear();
}

public interface IQuery<T> where T : class
{
    bool AsNoTracking { get; internal set; }
    bool AsSplitQuery { get; internal set; }
    bool IgnoreAutoIncludes { get; internal set; }
    bool IgnoreQueryFilters { get; internal set; }
    OrderBuilder<T> OrderBuilder { get; }
    Func<IEnumerable<T>, IEnumerable<T>> PostProcessingAction { get; internal set; }
    int? Skip { get; set; }
    int? Take { get; set; }

    SearchBuilder<T> SearchBuilder { get; }

    WhereBuilder<T> WhereBuilder { get; }

    void Clear();

    bool IsSatisfiedBy(T entity);

    void SetPage(int page, int pageSize);
}
