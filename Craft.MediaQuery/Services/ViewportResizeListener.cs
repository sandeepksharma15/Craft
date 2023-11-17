using Craft.MediaQuery.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Craft.MediaQuery.Services;

public class ViewportResizeListener(IJSRuntime jsRuntime,
    ILogger<ViewportResizeListener> logger,
    IOptions<ResizeOptions>? options = null)
        : IViewportResizeListener, IAsyncDisposable
{
    private readonly ResizeOptions _options = options?.Value ?? new ResizeOptions();
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask = new(()
        => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Craft.MediaQuery/resizeListener.js").AsTask());

    private bool _disposed;
    private EventHandler<ViewportSize> _onResized;

    public event EventHandler<ViewportSize> OnResized
    {
        add => Subscribe(value);
        remove => Unsubscribe(value);
    }

    public async ValueTask<ViewportSize> GetViewportSize()
    {
        logger.LogDebug("[ViewportResizeListener] GetViewportSize Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<ViewportSize>("getViewportSize");
    }

    public async ValueTask<bool> MatchMedia(string mediaQuery)
    {
        logger.LogDebug("[ViewportResizeListener] MatchMedia Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<bool>("matchMedia", mediaQuery);
    }

    [JSInvokable]
    public void RaiseOnResized(ViewportSize viewportSize)
    {
        logger.LogDebug($"[ViewportResizeListener] RaiseOnResized Invoked with Height: [{viewportSize.Height}] Width: [{viewportSize.Height}]");

        _onResized?.Invoke(this, viewportSize);
    }

    protected virtual void Dispose(bool disposing)
    {
        logger.LogDebug("[ViewportResizeListener] Dispose Invoked");

        if (!_disposed)
        {
            if (disposing)
                _onResized = null;

            _disposed = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
        logger.LogDebug("[ViewportResizeListener] DisposeAsync Invoked");
#pragma warning disable RCS1075 // Avoid empty catch clause that catches System.Exception.
        try
        {
            if (_moduleTask.IsValueCreated)
            {
                var module = await _moduleTask.Value;
                await module.DisposeAsync();
            }

            Dispose(true);
            GC.SuppressFinalize(this);
        }
        catch (Exception) { }
#pragma warning restore RCS1075 // Avoid empty catch clause that catches System.Exception.
    }

    private void Subscribe(EventHandler<ViewportSize>? value)
    {
        logger.LogDebug($"[ViewportResizeListener] Subscribe Invoked with value: [{value}]");

        if (_onResized is null)
            Task.Run(async () => await Start().ConfigureAwait(false));

        _onResized += value;
    }

    private void Unsubscribe(EventHandler<ViewportSize>? value)
    {
        logger.LogDebug($"[ViewportResizeListener] Unsubscribe Invoked with value: [{value}]");

        _onResized -= value;
        if (_onResized is null)
            Task.Run(async () => await Cancel().ConfigureAwait(false));
    }

    private async ValueTask<bool> Start()
    {
        logger.LogDebug("[ViewportResizeListener] Start Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<bool>("listenForResize", DotNetObjectReference.Create(this), _options);
    }

    private async ValueTask Cancel()
    {
        logger.LogDebug("[ViewportResizeListener] Cancel Invoked");

        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("cancelListener");
    }
}
