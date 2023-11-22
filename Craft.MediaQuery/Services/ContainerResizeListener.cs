using System;
using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;
using Craft.Utilities.Managers;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Craft.MediaQuery.Services;

public class ContainerResizeListener(IJSRuntime jsRuntime,
    ILogger<ViewportResizeListener> logger,
    IOptions<ResizeOptions>? options = null) : IContainerResizeListener, IAsyncDisposable
{
    private readonly SemaphoreSlim _semaphore = new(1, 5);
    private readonly ILogger<ViewportResizeListener> _logger = logger;
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask = new(() => jsRuntime
                .InvokeAsync<IJSObjectReference>("import", "./_content/Craft.MediaQuery/containerObserver.js")
                .AsTask());
    private readonly ObserverManager<ObserverSubscription, IContainerObserver> _observerManager = new(logger);

    public ResizeOptions ResizeOptions { get; } = options?.Value ?? new ResizeOptions();

    [JSInvokable]
    public Task RaiseOnResized(ViewportSize viewportSize, Breakpoint breakpoint, string elementId)
    {
        _logger.LogDebug("[ContainerResizeListener] OnResized Invoked");

        var resizeEventArgs = new ResizeEventArgs(viewportSize, breakpoint);

        return _observerManager.NotifyAsync(observer => observer.NotifyChangeAsync(resizeEventArgs),
            predicate: (subscription, _) => subscription.ElementId == elementId);
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask<Breakpoint> GetContainerBreakpointAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask<ViewportSize> GetContainerSizeAsync()
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> IsContainerBreakpointMatching(Breakpoint withBreakpoint)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> MatchContainerQuery(string mediaQuery)
    {
        throw new NotImplementedException();
    }

    public ValueTask<bool> MatchContainerQuery(int? minWidth = null, int? maxWidth = null)
    {
        throw new NotImplementedException();
    }

    public async Task SubscribeAsync(string elementId, IContainerObserver observer, bool fireImmediately = true)
    {
        try
        {
            await _semaphore.WaitAsync();

            var clonedOptions = (observer.ResizeOptions ?? ResizeOptions).Clone();
            clonedOptions.Breakpoints = GlobalOptions.GetBreakpoints(clonedOptions);

            var subscription = await CreateJsContainerObserver(elementId, clonedOptions, observer.Id);

            // If The Observer Is Not Already Subscribed, Fire Immediately
            if (!_observerManager.Observers.ContainsKey(subscription) && fireImmediately)
            {
                var viewportSize = await GetContainerSizeAsync();
                var breakpoint = await GetContainerBreakpointAsync();

                await observer.NotifyChangeAsync(new ResizeEventArgs(viewportSize, breakpoint));
            }

            // Either Subscribe Or ReSubscribe, As May Be The Case
            _observerManager.Subscribe(subscription, observer);
        }
        finally
        {
            _semaphore.Release();
        }
    }

    private async Task<ObserverSubscription> CreateJsContainerObserver(string elementId, ResizeOptions options, Guid id)
    {
        var observerId = _observerManager
            .Observers
            .Where(x => x.Key.ObserverId == id)
            .Select(x => x.Key.ObserverId)
            .FirstOrDefault();

        if (observerId == default)
        {
            var dotNetReference = DotNetObjectReference.Create(this);
            var module = await _moduleTask.Value;
            var result = await module.InvokeAsync<bool>("containerObserver", dotNetReference, options, elementId, id);

            if (!result)
                throw new ArgumentException($"Element with id {elementId} not found");

            return new ObserverSubscription(elementId, id, options);
        }

        return _observerManager.Observers.FirstOrDefault(x => x.Key.ObserverId == id).Key;
    }

    public Task UnsubscribeAsync(IContainerObserver observer)
    {
        throw new NotImplementedException();
    }
}
