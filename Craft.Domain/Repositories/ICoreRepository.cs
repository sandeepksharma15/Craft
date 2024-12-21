using Craft.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Craft.Domain.Repositories;

public interface ICoreRepository<T, TKey> : IRepository where T : class, IEntity<TKey>, new ()
{
    /// <summary>
    /// Returns the <see cref="DbContext"/> instance
    /// </summary>
    /// <returns></returns>
    Task<DbContext> GetDbContextAsync();

    /// <summary>
    /// Returns the <see cref="DbSet{T}"/> instance
    /// </summary>
    /// <returns></returns>
    Task<DbSet<T>> GetDbSetAsync();
}

public interface ICoreRepository<T> : ICoreRepository<T, KeyType> where T : class, IEntity, new ();
