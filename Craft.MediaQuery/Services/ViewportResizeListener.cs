using Craft.MediaQuery.Enums;
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
    private readonly ResizeOptions _options = options?.Value ?? GlobalOptions.GetDefaultResizeOptions();
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask = new(()
        => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Craft.MediaQuery/resizeListener.js").AsTask());

    private bool _disposed;
    private EventHandler<ResizeEventArgs> _onResized;
    private readonly string jsListenerId = Guid.NewGuid().ToString();

    public event EventHandler<ResizeEventArgs> OnResized
    {
        add => Subscribe(value);
        remove => Unsubscribe(value);
    }

    public async ValueTask<ViewportSize> GetViewportSize()
    {
        logger.LogDebug("[ViewportResizeListener] GetViewportSize Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<ViewportSize>("getViewportSize", jsListenerId);
    }

    public async ValueTask<Breakpoint> GetBreakpoint()
    {
        logger.LogDebug("[ViewportResizeListener] GetViewportSize Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<Breakpoint>("getBreakpoint", jsListenerId);
    }

    public async ValueTask<bool> MatchMedia(string mediaQuery)
    {
        logger.LogDebug("[ViewportResizeListener] MatchMedia Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<bool>("matchMediaQuery", mediaQuery, jsListenerId);
    }

    [JSInvokable]
    public void RaiseOnResized(ViewportSize viewportSize, Breakpoint breakpoint)
    {
        logger.LogDebug($"[ViewportResizeListener] RaiseOnResized Invoked with Height: [{viewportSize.Height}] Width: [{viewportSize.Height}]");
        logger.LogDebug($"[ViewportResizeListener] RaiseOnResized Invoked with Breakpoint: [{breakpoint}]");

        _onResized?.Invoke(this, new ResizeEventArgs(viewportSize, breakpoint, false));
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

    private void Subscribe(EventHandler<ResizeEventArgs>? value)
    {
        logger.LogDebug($"[ViewportResizeListener] Subscribe Invoked with value: [{value}]");

        if (_onResized is null)
            Task.Run(async () => await Start().ConfigureAwait(false));

        _onResized += value;
    }

    private void Unsubscribe(EventHandler<ResizeEventArgs>? value)
    {
        logger.LogDebug($"[ViewportResizeListener] Unsubscribe Invoked with value: [{value}]");

        _onResized -= value;
        if (_onResized is null)
            Task.Run(async () => await Cancel().ConfigureAwait(false));
    }

    private async ValueTask<bool> Start()
    {
        logger.LogDebug("[ViewportResizeListener] Start Invoked");

        var optionsClone = _options.Clone();
        optionsClone.Breakpoints = GlobalOptions.GetBreakpoints(optionsClone);

        // TODO: Remove this later
        optionsClone.EnableLogging = true;

        var module = await _moduleTask.Value;

        try
        {
            await module.InvokeVoidAsync("resizeListener", DotNetObjectReference.Create(this), optionsClone, jsListenerId);

            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[ViewportResizeListener] Start Failed");
            return false;
        }
    }

    private async ValueTask Cancel()
    {
        logger.LogDebug("[ViewportResizeListener] Cancel Invoked");

        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("cancelListener", jsListenerId);
    }
}
