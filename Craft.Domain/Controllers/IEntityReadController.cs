using Craft.Domain.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Craft.Domain.Controllers;

public interface IEntityReadController<T, TKey> : IEntityController where T : class, IEntity<TKey>, new()
{
    Task<ActionResult<IAsyncEnumerable<T>>> GetAllAsync(bool includeDetails, CancellationToken cancellationToken = default);

    Task<ActionResult<T>> GetAsync(TKey id, bool includeDetails, CancellationToken cancellationToken = default);

    Task<ActionResult<long>> GetCountAsync(CancellationToken cancellationToken = default);
}

public interface IEntityReadController<T> : IEntityReadController<T, KeyType> where T : class, IEntity, new();
