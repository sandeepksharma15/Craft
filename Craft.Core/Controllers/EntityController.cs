using Craft.Domain.Contracts;
using Craft.Domain.Controllers;
using Craft.Domain.Helpers;
using Craft.Domain.Repositories;
using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Craft.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class EntityController<T, DataTransferT, TKey>(IRepository<T, TKey> repository, ILogger<EntityController<T, DataTransferT, TKey>> logger)
    : EntityChangeController<T, DataTransferT, TKey>(repository, logger), IEntityController<T, DataTransferT, TKey>
        where T : class, IEntity<TKey>, new()
        where DataTransferT : class, IModel<TKey>, new()
{
    [HttpPost("delete")]
    public virtual async Task<ActionResult> DeleteAsync(IQuery<T> query, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[EntityController] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"DeleteAsync\"]");

        try
        {
            await _repository.DeleteAsync(query, cancellationToken: cancellationToken);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return Problem(ex.Message);
        }
    }

    [HttpPost("find")]
    public virtual async Task<ActionResult<T>> GetAsync(IQuery<T> query, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[EntityController] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetAsync\"]");

        try
        {
            T entity = await _repository.GetAsync(query, cancellationToken);

            return entity == null ? NotFound() : Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return Problem(ex.Message);
        }
    }

    [HttpPost("selectone")]
    public virtual async Task<ActionResult<TResult>> GetAsync<TResult>(IQuery<T, TResult> query, CancellationToken cancellationToken = default)
        where TResult : class, new()
    {
        _logger.LogDebug($"[EntityController] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetAsync\"]");

        try
        {
            TResult entity = await _repository.GetAsync<TResult>(query, cancellationToken);

            return entity == null ? NotFound() : Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return Problem(ex.Message);
        }
    }

    [HttpPost("filtercount")]
    public virtual async Task<ActionResult<long>> GetCountAsync(IQuery<T> query, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[EntityController] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetAsync\"]");

        try
        {
            return Ok(await _repository.GetCountAsync(query, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return Problem(ex.Message);
        }
    }

    [HttpPost("search")]
    public virtual async Task<ActionResult<PageResponse<T>>> GetPagedListAsync([FromBody] IQuery<T> query, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[EntityController] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetPagedListAsync\"]");

        try
        {
            return Ok(await _repository.GetPagedListAsync(query, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return Problem(ex.Message);
        }
    }

    [HttpPost("select")]
    public virtual async Task<ActionResult<PageResponse<TResult>>> GetPagedListAsync<TResult>(IQuery<T, TResult> query, CancellationToken cancellationToken = default)
        where TResult : class, new()
    {
        _logger.LogDebug($"[EntityController] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetPagedListAsync\"]");

        try
        {
            return Ok(await _repository.GetPagedListAsync(query, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return Problem(ex.Message);
        }
    }
}

public abstract class EntityController<T, DataTransferT>(IRepository<T> repository, ILogger<EntityController<T, DataTransferT>> logger)
    : EntityController<T, DataTransferT, KeyType>(repository, logger), IEntityController<T, DataTransferT>
        where T : class, IEntity, new()
        where DataTransferT : class, IModel, new() { }
