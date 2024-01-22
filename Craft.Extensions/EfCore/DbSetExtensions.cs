using System.Linq.Expressions;
using Craft.Extensions.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore;

public static class DbSetExtensions
{
    /// <summary>
    /// Retrieves the query filter expression that has been configured for a given entity type within the current model.
    /// Enables the retrieval and application of global query filters defined for entities.
    /// </summary>
    /// <typeparam name="T">The type of the entity for which to retrieve the query filter.</typeparam>
    /// <param name="dbSet">The DbSet representing the entity type.</param>
    /// <returns>The Expression<Func<T, bool>> representing the query filter, or null if no filter is configured for the entity type.</returns>
    public static Expression<Func<T, bool>> GetQueryFilter<T>(this DbSet<T> dbSet) where T : class
    {
        var entityType = dbSet.GetType().GenericTypeArguments[0];
        var model = dbSet.GetService<IModel>();
        var entityTypeConfiguration = model.FindEntityType(entityType);

        return (Expression<Func<T, bool>>)entityTypeConfiguration.GetQueryFilter();
    }

    /// <summary>
    /// Conditionally includes or excludes automatically included navigation properties in a query.
    /// Provides flexibility in controlling the eager loading behavior of related data.
    /// </summary>
    /// <typeparam name="T">The type of entity in the queryable.</typeparam>
    /// <param name="source">The source IQueryable<TEntity> to potentially modify.</param>
    /// <param name="includeDetails">A boolean indicating whether to include or exclude automatically included navigation properties.</param>
    /// <returns>The modified IQueryable<TEntity> with navigation properties included or excluded based on the includeDetails parameter.</returns>
    public static IQueryable<T> IncludeDetails<T>(this IQueryable<T> source, bool includeDetails) where T : class
        => (includeDetails) ? source : source.IgnoreAutoIncludes();

    /// <summary>
    /// Removes a specific condition from the existing query filter for a given DbSet, if present.
    /// Allows for conditional removal of filters to control query results dynamically.
    /// </summary>
    /// <typeparam name="T">The type of the entity in the DbSet.</typeparam>
    /// <param name="dbSet">The DbSet to modify the query filter for.</param>
    /// <param name="condition">The condition to remove from the query filter.</param>
    /// <returns>An IQueryable<T> with the modified query filter, either using IgnoreQueryFilters if the filter is removed entirely, or applying the updated filter.</returns>
    public static IQueryable<T> RemoveFromQueryFilter<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> condition) where T : class
    {
        var queryFilter = dbSet.GetQueryFilter();
        var newQueryFilter = queryFilter.RemoveCondition(condition);

        return (newQueryFilter == null)
            ? dbSet.IgnoreQueryFilters().AsQueryable()
            : dbSet.IgnoreQueryFilters().Where(newQueryFilter);
    }
}
