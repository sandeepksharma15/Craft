using System.Linq.Expressions;
using Craft.Extensions.Expressions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Microsoft.EntityFrameworkCore;

public static class DbSetExtensions
{
    public static Expression<Func<T, bool>> GetQueryFilter<T>(this DbSet<T> dbSet) where T : class
    {
        var entityType = dbSet.GetType().GenericTypeArguments[0];
        var model = dbSet.GetService<IModel>();
        var entityTypeConfiguration = model.FindEntityType(entityType);

        return (Expression<Func<T, bool>>)entityTypeConfiguration.GetQueryFilter();
    }

    public static IQueryable<TEntity> IncludeDetails<TEntity>(this IQueryable<TEntity> source, bool includeDetails)
       where TEntity : class
    {
        return (includeDetails) ? source : source.IgnoreAutoIncludes();
    }

    public static IQueryable<T> RemoveFromQueryFilter<T>(this DbSet<T> dbSet, Expression<Func<T, bool>> condition) where T : class
    {
        var queryFilter = dbSet.GetQueryFilter();
        var newQueryFilter = queryFilter.RemoveCondition(condition);

        return (newQueryFilter == null)
            ? dbSet.IgnoreQueryFilters().AsQueryable()
            : dbSet.IgnoreQueryFilters().Where(newQueryFilter);
    }
}
