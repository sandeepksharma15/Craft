﻿using Craft.QuerySpec.Contracts;

namespace Craft.QuerySpec.Core;

public static class QueryExtensions
{
    public static IQuery<T> AsNoTracking<T>(this IQuery<T> query) where T : class
    {
        if (query is null) return null;

        query.AsNoTracking = true;
        return query;
    }

    public static IQuery<T> AsSplitQuery<T>(this IQuery<T> query) where T : class
    {
        if (query is null) return null;

        query.AsSplitQuery = true;
        return query;
    }

    public static IQuery<T> IgnoreQueryFilters<T>(this IQuery<T> query) where T : class
    {
        if (query is null) return null;

        query.IgnoreQueryFilters = true;
        return query;
    }

    public static IQuery<T> Skip<T>(this IQuery<T> query, int? skip) where T : class
    {
        if (query is null) return null;

        query.Skip = skip;
        return query;
    }

    public static IQuery<T> Take<T>(this IQuery<T> query, int? take) where T : class
    {
        if (query is null) return null;

        query.Take = take;
        return query;
    }

    public static IQuery<T> PostProcessingAction<T>(this IQuery<T> query, Func<IEnumerable<T>, IEnumerable<T>> postProcessingAction) where T : class
    {
        if (query is null) return null;

        query.PostProcessingAction = postProcessingAction;
        return query;
    }

    public static IQuery<T, TResult> PostProcessingAction<T, TResult>(this IQuery<T, TResult> query, Func<IEnumerable<TResult>, IEnumerable<TResult>> postProcessingAction) where T : class where TResult : class
    {
        if (query is null) return null;

        query.PostProcessingAction = postProcessingAction;
        return query;
    }

    public static bool IsWithoutOrder<T>(this IQuery<T> query) where T : class
    {
        if (query is null) return true;

        return query.OrderBuilder is null || query.OrderBuilder.OrderDescriptorList.Count == 0;
    }
}
