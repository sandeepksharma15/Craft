using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;
using Craft.Utilities.Managers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Craft.MediaQuery.Services;

public class ContainerResizeListener : IContainerResizeListener, IAsyncDisposable
{
    private readonly SemaphoreSlim _semaphore;
    private readonly ILogger<ContainerResizeListener> _logger;
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    private readonly ObserverManager<ObserverSubscription, IContainerObserver> _observerManager;

    public ResizeOptions ResizeOptions { get; }

    public ContainerResizeListener(IJSRuntime jsRuntime, ILogger<ContainerResizeListener> logger, IOptions<ResizeOptions>? options = null)
    {
        _semaphore = new(1, 5);
        _logger = logger;

        _moduleTask = new(() => jsRuntime
                .InvokeAsync<IJSObjectReference>("import", "./_content/Craft.MediaQuery/containerObserver.js")
                .AsTask());

        _observerManager = new(logger);

        ResizeOptions = options?.Value ?? new ResizeOptions();
    }

    [JSInvokable]
    public Task RaiseOnResized(ViewportSize viewportSize, Breakpoint breakpoint, string elementId)
    {
        _logger.LogDebug("[ContainerResizeListener] OnResized Invoked");

        var resizeEventArgs = new ContainerResizeEventArgs(elementId, viewportSize, breakpoint);

        _logger.LogDebug($"Element Id: {elementId} Breakpoint: {breakpoint}");

        return _observerManager.NotifyAsync(observer => observer.NotifyChangeAsync(resizeEventArgs),
            predicate: (subscription, _) => subscription.ElementId == elementId);
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogDebug("[ContainerResizeListener] DisposeAsync Invoked");

        try
        {
            var elementIds = _observerManager.Observers.Select(x => x.Key.ElementId).ToList();

            if (elementIds.Count > 0)
            {
                var module = await _moduleTask.Value;
                await module.InvokeVoidAsync("removeAllObservers", elementIds);
            }

            _observerManager.Clear();

            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }

            GC.SuppressFinalize(this);
        }
        catch (Exception)
        {
            _logger.LogDebug("[ContainerResizeListener] DisposeAsync Resulted In an Error!");
        }
    }

    public async ValueTask<Breakpoint> GetContainerBreakpointAsync(string elementId)
    {
        _logger.LogDebug("[ContainerResizeListener] GetContainerBreakpointAsync Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<Breakpoint>("getContainerBreakpoint", elementId);
    }

    public async ValueTask<ViewportSize> GetContainerSizeAsync(string elementId)
    {
        _logger.LogDebug("[ContainerResizeListener] GetContainerSizeAsync Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<ViewportSize>("getContainerSize", elementId);
    }

    public async ValueTask<bool> IsContainerBreakpointMatchingAsync(string elementId, Breakpoint withBreakpoint)
    {
        var breakpoint = await GetContainerBreakpointAsync(elementId);

        return breakpoint.IsMatchingWith(withBreakpoint);
    }

    public async ValueTask<bool> MatchContainerQueryAsync(string elementId, string containerQuery)
    {
        _logger.LogDebug("[ContainerResizeListener] MatchContainerQuery Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<bool>("matchContainerQuery", containerQuery, elementId);
    }

    public async ValueTask<bool> MatchContainerQueryAsync(string elementId, int? minWidth = null, int? maxWidth = null,
        int? minHeight = null, int? maxHeight = null)
    {
        //if (minWidth is not null && maxWidth is not null)
        //    return await MatchContainerQueryAsync(elementId, $"(min-width: {minWidth}px) and (max-width: {maxWidth}px)");

        //if (minWidth is not null)
        //    return await MatchContainerQueryAsync(elementId, $"(min-width: {minWidth}px)");

        //if (maxWidth is not null)
        //    return await MatchContainerQueryAsync(elementId, $"(max-width: {maxWidth}px)");

        return false;
    }

    public async Task SubscribeAsync(string elementId, IContainerObserver observer, bool fireImmediately = true)
    {
        try
        {
            await _semaphore.WaitAsync();

            var clonedOptions = (observer.ResizeOptions ?? ResizeOptions).Clone();
            clonedOptions.Breakpoints = GlobalOptions.GetBreakpoints(clonedOptions);

            var subscription = await CreateJsContainerObserver(elementId, clonedOptions);

            if (subscription is not null)
            {
                // If The Observer Is Not Already Subscribed, Fire Immediately
                if (!_observerManager.Observers.ContainsKey(subscription) && fireImmediately)
                {
                    var viewportSize = await GetContainerSizeAsync(elementId);
                    var breakpoint = await GetContainerBreakpointAsync(elementId);

                    await observer.NotifyChangeAsync(
                        new ContainerResizeEventArgs(elementId, viewportSize, breakpoint));
                }

                // Either Subscribe Or ReSubscribe, As May Be The Case
                _observerManager.Subscribe(subscription, observer);
            }
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<ObserverSubscription> CreateJsContainerObserver(string elementId, ResizeOptions options)
    {
        bool result = false;

        var id = _observerManager
            .Observers
            .Where(x => x.Key.ElementId == elementId)
            .Select(x => x.Key.ElementId)
            .FirstOrDefault();

        if (id == default)
        {
            var dotNetReference = DotNetObjectReference.Create(this);

            try
            {
                var module = await _moduleTask.Value;
                result = await module.InvokeAsync<bool>("containerObserver", dotNetReference, options, elementId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ContainerResizeListener] CreateJsContainerObserver Failed");
                return null;
            }

            if (!result)
                throw new ArgumentException($"Element with id {elementId} not found");

            return new ObserverSubscription(elementId, Guid.NewGuid(), options);
        }

        return _observerManager.Observers.FirstOrDefault(x => x.Key.ElementId == elementId).Key;
    }

    public async Task UnsubscribeAsync(string elementId)
    {
        try
        {
            await _semaphore.WaitAsync();

            var subscrption = await RemoveJsCoontainerObserver(elementId);

            if (subscrption is not null)
                _observerManager.Unsubscribe(subscrption);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<ObserverSubscription> RemoveJsCoontainerObserver(string elementId)
    {
        var subscription = _observerManager
            .Observers
            .Select(x => x.Key)
            .FirstOrDefault(x => x.ElementId == elementId);

        if (subscription is null)
            return null;

        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("removeContainerObserver", elementId);

        return subscription;
    }
}
