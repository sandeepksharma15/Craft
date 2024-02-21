using System.Linq.Expressions;
using Craft.QuerySpec.Contracts;

namespace Craft.QuerySpec.Core;

public static class QuerySelectExtensions
{
    public static IQuery<T, TResult> Select<T, TResult>(this IQuery<T, TResult> query, Expression<Func<T, object>> column)
        where T : class
        where TResult : class
    {
        if (query is null) return null;
        if (column is null) return query;

        query.QuerySelectBuilder.Add(column);

        return query;
    }

    public static IQuery<T, TResult> Select<T, TResult>(this IQuery<T, TResult> query, Expression<Func<T, object>> assignor, Expression<Func<TResult, object>> assignee)
        where T : class
        where TResult : class
    {
        if (query is null) return null;
        if (assignor is null) return query;
        if (assignee is null) return query;

        query.QuerySelectBuilder.Add(assignor, assignee);

        return query;
    }

    public static IQuery<T, TResult> Select<T, TResult>(this IQuery<T, TResult> query, string assignorPropName)
        where T : class
        where TResult : class
    {
        if (query is null) return null;
        if (assignorPropName is null) return query;

        query.QuerySelectBuilder.Add(assignorPropName);

        return query;
    }

    public static IQuery<T, TResult> Select<T, TResult>(this IQuery<T, TResult> query, string assignorPropName, string assigneePropName)
        where T : class
        where TResult : class
    {
        if (query is null) return null;
        if (assignorPropName is null) return query;
        if (assigneePropName is null) return query;

        query.QuerySelectBuilder.Add(assignorPropName, assigneePropName);

        return query;
    }

    public static IQuery<T, TResult> SelectMany<T, TResult>(this IQuery<T, TResult> query, Expression<Func<T, IEnumerable<TResult>>> selector)
        where T : class
        where TResult : class
    {
        if (query is null) return null;
        if (selector is null) return query;

        query.SelectorMany = selector;

        return query;
    }
}
