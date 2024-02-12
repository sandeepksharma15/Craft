using System.Linq.Expressions;
using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Enums;

namespace Craft.QuerySpec.Core;

public static class QueryWhereExtensions
{
    public static IQuery<T> Where<T>(this IQuery<T> query, Expression<Func<T, bool>> expression) where T : class
    {
        if (query is null) return null;
        if (expression is null) return query;

        query.WhereBuilder.Add(expression);

        return query;
    }

    public static IQuery<T> Where<T>(this IQuery<T> query, Expression<Func<T, object>> propExpr,
        object compareWith, ComparisonType comparisonType = ComparisonType.EqualTo) where T : class
    {
        if (query is null) return null;
        if (propExpr is null) return query;

        query.WhereBuilder.Add(propExpr, compareWith, comparisonType);

        return query;
    }

    public static IQuery<T> Where<T>(this IQuery<T> query, string propName,
        object compareWith, ComparisonType comparisonType = ComparisonType.EqualTo) where T : class
    {
        if (query is null) return null;
        if (propName is null) return query;

        query.WhereBuilder.Add(propName, compareWith, comparisonType);

        return query;
    }
}
