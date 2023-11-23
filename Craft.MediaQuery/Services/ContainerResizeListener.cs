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
    private readonly ILogger<ViewportResizeListener> _logger;
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    private readonly ObserverManager<ObserverSubscription, IContainerObserver> _observerManager;

    public ResizeOptions ResizeOptions { get; }

    public ContainerResizeListener(IJSRuntime jsRuntime, ILogger<ViewportResizeListener> logger, IOptions<ResizeOptions>? options = null)
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

        var resizeEventArgs = new ResizeEventArgs(viewportSize, breakpoint);

        return _observerManager.NotifyAsync(observer => observer.NotifyChangeAsync(resizeEventArgs),
            predicate: (subscription, _) => subscription.ElementId == elementId);
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogDebug("[ContainerResizeListener] DisposeAsync Invoked");

        try
        {
            // Get All Observer Ids
            var observerIds = _observerManager.Observers.Select(x => x.Key.ObserverId).ToList();

            if (observerIds.Count > 0)
            {
                var module = await _moduleTask.Value;
                await module.InvokeVoidAsync("removeObservers", observerIds);
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

    public async ValueTask<Breakpoint> GetContainerBreakpointAsync(Guid observerId)
    {
        _logger.LogDebug("[ContainerResizeListener] GetContainerBreakpointAsync Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<Breakpoint>("getContainerBreakpoint", observerId);
    }

    public async ValueTask<ViewportSize> GetContainerSizeAsync(Guid observerId)
    {
        _logger.LogDebug("[ContainerResizeListener] GetContainerSizeAsync Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<ViewportSize>("getContainerSize", observerId);
    }

    public async ValueTask<bool> IsContainerBreakpointMatchingAsync(Guid observerId, Breakpoint withBreakpoint)
    {
        var breakpoint = await GetContainerBreakpointAsync(observerId);

        return breakpoint.IsMatchingWith(withBreakpoint);
    }

    public async ValueTask<bool> MatchContainerQueryAsync(Guid observerId, string containerQuery)
    {
        _logger.LogDebug("[ContainerResizeListener] MatchContainerQuery Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<bool>("matchContainerQuery", containerQuery, observerId);
    }

    public async ValueTask<bool> MatchContainerQueryAsync(Guid observerId, int? minWidth = null, int? maxWidth = null)
    {
        if (minWidth is not null && maxWidth is not null)
            return await MatchContainerQueryAsync(observerId, $"(min-width: {minWidth}px) and (max-width: {maxWidth}px)");

        if (minWidth is not null)
            return await MatchContainerQueryAsync(observerId, $"(min-width: {minWidth}px)");

        if (maxWidth is not null)
            return await MatchContainerQueryAsync(observerId, $"(max-width: {maxWidth}px)");

        return false;
    }

    public async Task SubscribeAsync(string elementId, IContainerObserver observer, bool fireImmediately = true)
    {
        try
        {
            await _semaphore.WaitAsync();

            var clonedOptions = (observer.ResizeOptions ?? ResizeOptions).Clone();
            clonedOptions.Breakpoints = GlobalOptions.GetBreakpoints(clonedOptions);

            var subscription = await CreateJsContainerObserver(elementId, clonedOptions, observer.Id);

            if (subscription is not null)
            {
                // If The Observer Is Not Already Subscribed, Fire Immediately
                if (!_observerManager.Observers.ContainsKey(subscription) && fireImmediately)
                {
                    var viewportSize = await GetContainerSizeAsync(observer.Id);
                    var breakpoint = await GetContainerBreakpointAsync(observer.Id);

                    await observer.NotifyChangeAsync(new ResizeEventArgs(viewportSize, breakpoint));
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

    private async Task<ObserverSubscription> CreateJsContainerObserver(string elementId, ResizeOptions options, Guid id)
    {
        bool result = false;

        var observerId = _observerManager
            .Observers
            .Where(x => x.Key.ObserverId == id)
            .Select(x => x.Key.ObserverId)
            .FirstOrDefault();

        if (observerId == default)
        {
            var dotNetReference = DotNetObjectReference.Create(this);

            try
            {
                var module = await _moduleTask.Value;
                result = await module.InvokeAsync<bool>("containerObserver", dotNetReference, options, elementId, id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[ContainerResizeListener] CreateJsContainerObserver Failed");
                return null;
            }

            if (!result)
                throw new ArgumentException($"Element with id {elementId} not found");

            return new ObserverSubscription(elementId, id, options);
        }

        return _observerManager.Observers.FirstOrDefault(x => x.Key.ObserverId == id).Key;
    }

    public async Task UnsubscribeAsync(Guid observerId)
    {
        try
        {
            await _semaphore.WaitAsync();

            var subscrption = await RemoveJsCoontainerObserver(observerId);

            if (subscrption is not null)
                _observerManager.Unsubscribe(subscrption);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<ObserverSubscription> RemoveJsCoontainerObserver(Guid observerId)
    {
        var subscription = _observerManager
            .Observers
            .Select(x => x.Key)
            .FirstOrDefault(x => x.ObserverId == observerId);

        if (subscription is null)
            return null;

        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("removeObserver", observerId);

        return subscription;
    }
}
