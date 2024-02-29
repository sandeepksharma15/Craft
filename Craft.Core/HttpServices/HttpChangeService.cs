using System.Net.Http.Json;
using Craft.Domain.Contracts;
using Craft.Domain.HttpServices;
using Mapster;
using Microsoft.Extensions.Logging;

namespace Craft.Core.HttpServices;

public class HttpChangeService<T, ViewT, DataTransferT, TKey>(Uri apiURL, HttpClient httpClient, ILogger<HttpChangeService<T, ViewT, DataTransferT, TKey>> logger)
    : HttpReadService<T, TKey>(apiURL, httpClient, logger), IHttpChangeService<T, ViewT, DataTransferT, TKey>
        where T : class, IEntity<TKey>, IModel<TKey>
        where ViewT : class, IModel<TKey>
        where DataTransferT : class, IModel<TKey>
{
    public virtual async Task<HttpResponseMessage> AddAsync(ViewT model, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[HttpChangeService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"AddAsync\"]");

        DataTransferT dto = model.Adapt<DataTransferT>();

        return await _httpClient
            .PostAsJsonAsync(_apiURL, dto, cancellationToken: cancellationToken);
    }

    public virtual async Task<HttpResponseMessage> AddRangeAsync(IEnumerable<ViewT> models, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[HttpChangeService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"AddRangeAsync\"]");

        IEnumerable<DataTransferT> dtos = models.Adapt<IEnumerable<DataTransferT>>();

        return await _httpClient
            .PostAsJsonAsync(new Uri($"{_apiURL}/addrange"), dtos, cancellationToken: cancellationToken);
    }

    public virtual async Task<HttpResponseMessage> DeleteAsync(TKey id, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[HttpChangeService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"DeleteAsync\"]");

        return await _httpClient
            .DeleteAsync(new Uri($"{_apiURL}/{id}"), cancellationToken);
    }

    public virtual async Task<HttpResponseMessage> DeleteRangeAsync(IEnumerable<ViewT> models, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[HttpChangeService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"DeleteRangeAsync\"]");

        IEnumerable<DataTransferT> dtos = models.Adapt<IEnumerable<DataTransferT>>();

        return await _httpClient
            .PutAsJsonAsync(new Uri($"{_apiURL}/RemoveRange"), dtos, cancellationToken: cancellationToken);
    }

    public virtual async Task<HttpResponseMessage> UpdateAsync(ViewT model, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[HttpChangeService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"UpdateAsync\"]");

        DataTransferT dto = model.Adapt<DataTransferT>();

        return await _httpClient.PutAsJsonAsync(_apiURL, dto, cancellationToken: cancellationToken);
    }

    public virtual async Task<HttpResponseMessage> UpdateRangeAsync(IEnumerable<ViewT> models, CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"[HttpChangeService] Type: [\"{typeof(T).GetClassName()}\"] Method: [\"UpdateRangeAsync\"]");

        IEnumerable<DataTransferT> dtos = models.Adapt<IEnumerable<DataTransferT>>();

        return await _httpClient
            .PutAsJsonAsync(new Uri($"{_apiURL}/UpdateRange"), dtos, cancellationToken: cancellationToken);
    }
}

public class HttpChangeService<T, ViewT, DataTransferT>(Uri apiURL, HttpClient httpClient, ILogger<HttpChangeService<T, ViewT, DataTransferT>> logger)
    : HttpChangeService<T, ViewT, DataTransferT, KeyType>(apiURL, httpClient, logger), IHttpChangeService<T, ViewT, DataTransferT>
        where T : class, IEntity, IModel
        where ViewT : class, IModel
        where DataTransferT : class, IModel { }
