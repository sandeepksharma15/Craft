using Craft.Domain.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Craft.Domain.Repositories;

public interface ICoreRepository<T, TKey> : IRepository where T : class, IEntity<TKey>, new ()
{
    Task<DbContext> GetDbContextAsync();
    Task<DbSet<T>> GetDbSetAsync();
}

public interface ICoreRepository<T> : ICoreRepository<T, KeyType> where T : class, IEntity, new ();
