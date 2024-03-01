using Craft.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Craft.Data.Extensions;

public static class DbSetExtensions
{
    public static IQueryable<T> IgnoreSoftDelete<T>(this DbSet<T> dbSet) where T : class, ISoftDelete, new()
    {
        return dbSet.RemoveFromQueryFilter(s => !s.IsDeleted);
    }
}
