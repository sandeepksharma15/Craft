using Craft.Domain.Contracts;
using Craft.Domain.Controllers;
using Craft.Domain.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Craft.Core.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class EntityReadController<T, DataTransferT, TKey>(IRepository<T, TKey> repository, ILogger<EntityReadController<T, DataTransferT, TKey>> logger)
    : ControllerBase, IEntityReadController<T, TKey> where T : class, IEntity<TKey>, new()
        where DataTransferT : class, IModel<TKey>, new()

{
    protected readonly ILogger<EntityReadController<T, DataTransferT, TKey>> _logger = logger;
    protected readonly IRepository<T, TKey> _repository = repository;

    [HttpGet("{includeDetails:bool}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<ActionResult<IAsyncEnumerable<T>>> GetAllAsync(bool includeDetails, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[EntityReadController] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetAllAsync\"]");

        try
        {
            return Ok(await _repository.GetAllAsync(includeDetails, cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return Problem(ex.Message);
        }
    }

    [HttpGet("{id}/{includeDetails:bool}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<ActionResult<T>> GetAsync(TKey id, bool includeDetails, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[EntityReadController] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetAsync\"] Id: [\"{id}\"]");

        try
        {
            T entity = await _repository.GetAsync(id, includeDetails, cancellationToken);

            return entity == null ? NotFound() : Ok(entity);
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    [Route("count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public virtual async Task<ActionResult<long>> GetCountAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[EntityReadController] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"GetCountAsync\"]");

        try
        {
            return Ok(await _repository.GetCountAsync(cancellationToken));
        }
        catch (Exception ex)
        {
            _logger.LogCritical(ex.Message);
            return Problem(ex.Message);
        }
    }
}

public abstract class EntityReadController<T, DataTransferT>(IRepository<T> repository, ILogger<EntityReadController<T, DataTransferT>> logger)
    : EntityReadController<T, DataTransferT, KeyType>(repository, logger), IEntityReadController<T>
        where T : class, IEntity, new()
        where DataTransferT : class, IModel, new() { }
