using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Craft.Data.Extensions;

public static class QueryFilterExtension
{
    public static ModelBuilder AddGlobalQueryFilter<T>(this ModelBuilder modelBuilder, Expression<Func<T, bool>> filter)
    {
        if (!typeof(T).IsInterface)
            throw new ArgumentException("Type argument must be an interface");

        ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));
        ArgumentNullException.ThrowIfNull(filter, nameof(filter));

        // Get List Of Entities That Implement The Interface TInterface
        var entities = modelBuilder
            .Model
            .GetEntityTypes()
            .Where(e => e.ClrType.GetInterface(typeof(T).Name) is not null)
            .Select(e => e.ClrType);

        foreach (var entity in entities)
        {
            var parameterType = Expression.Parameter(modelBuilder.Entity(entity).Metadata.ClrType);
            var filterBody = ReplacingExpressionVisitor.Replace(filter.Parameters.Single(), parameterType, filter.Body);

            // Get The Existing Query Filter
            if (modelBuilder.Entity(entity).Metadata.GetQueryFilter() is { } existingFilter)
            {
                var existingFilterBody = ReplacingExpressionVisitor.Replace(existingFilter.Parameters.Single(), parameterType, existingFilter.Body);

                // Append The New Query To The Query Filter
                filterBody = Expression.AndAlso(existingFilterBody, filterBody);
            }

            // Apply The New Query Filter
            modelBuilder.Entity(entity).HasQueryFilter(Expression.Lambda(filterBody, parameterType));
        }

        return modelBuilder;
    }
}
