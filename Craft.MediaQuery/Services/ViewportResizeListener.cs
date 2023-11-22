using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Craft.MediaQuery.Services;

public class ViewportResizeListener : IViewportResizeListener, IAsyncDisposable
{
    private readonly ResizeOptions _options;
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    private bool _disposed;
    private EventHandler<ResizeEventArgs> _onResized;
    private readonly string jsListenerId = Guid.NewGuid().ToString();
    private readonly ILogger<ViewportResizeListener> _logger;
    private Breakpoint _lastBreakpoint = Breakpoint.None;

    public ViewportResizeListener(IJSRuntime jsRuntime,
        ILogger<ViewportResizeListener> logger,
        IOptions<ResizeOptions>? options = null)
    {
        _logger = logger;

        _moduleTask = new(()
            => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Craft.MediaQuery/resizeListener.js").AsTask());

        // Get the user options and set Defaults if not provided
        _options = options?.Value ?? GlobalOptions.GetDefaultResizeOptions();
        _options.Breakpoints = GlobalOptions.GetBreakpoints(_options);
    }

    public event EventHandler<ResizeEventArgs> OnResized
    {
        add => Subscribe(value);
        remove => Unsubscribe(value);
    }

    public async ValueTask<ViewportSize> GetViewportSizeAsync()
    {
        _logger.LogDebug("[ViewportResizeListener] GetViewportSize Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<ViewportSize>("getViewportSize", jsListenerId);
    }

    public async ValueTask<Breakpoint> GetBreakpointAsync()
    {
        _logger.LogDebug("[ViewportResizeListener] GetViewportSize Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<Breakpoint>("getBreakpoint", jsListenerId);
    }

    public async ValueTask<bool> MatchMediaAsync(string mediaQuery)
    {
        _logger.LogDebug("[ViewportResizeListener] MatchMedia Invoked");

        var module = await _moduleTask.Value;
        return await module.InvokeAsync<bool>("matchMediaQuery", mediaQuery, jsListenerId);
    }

    public async ValueTask<bool> IsBreakpointMatchingAsync(Breakpoint withBreakpoint)
    {
        if (_lastBreakpoint == Breakpoint.None)
            _lastBreakpoint = await GetBreakpointAsync();

        return _lastBreakpoint.IsMatchingWith(withBreakpoint);
    }

    public async ValueTask<bool> MatchMediaAsync(int? minWidth = null, int? maxWidth = null)
    {
        if (minWidth is not null && maxWidth is not null)
            return await MatchMediaAsync($"(min-width: {minWidth}px) and (max-width: {maxWidth}px)");

        if (minWidth is not null)
            return await MatchMediaAsync($"(min-width: {minWidth}px)");

        if (maxWidth is not null)
            return await MatchMediaAsync($"(max-width: {maxWidth}px)");

        return false;
    }

    [JSInvokable]
    public void RaiseOnResized(ViewportSize viewportSize, Breakpoint breakpoint)
    {
        _logger.LogDebug($"[ViewportResizeListener] RaiseOnResized Invoked with Height: [{viewportSize.Height}] Width: [{viewportSize.Height}]");
        _logger.LogDebug($"[ViewportResizeListener] RaiseOnResized Invoked with Breakpoint: [{breakpoint}]");

        // Cache this as we may use it later
        _lastBreakpoint = breakpoint;

        _onResized?.Invoke(this, new ResizeEventArgs(viewportSize, breakpoint, false));
    }

    protected virtual void Dispose(bool disposing)
    {
        _logger.LogDebug("[ViewportResizeListener] Dispose Invoked");

        if (!_disposed)
        {
            if (disposing)
                _onResized = null;

            _disposed = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogDebug("[ViewportResizeListener] DisposeAsync Invoked");
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
        _logger.LogDebug($"[ViewportResizeListener] Subscribe Invoked with value: [{value}]");

        if (_onResized is null)
            Task.Run(async () => await Start().ConfigureAwait(false));

        _onResized += value;
    }

    private void Unsubscribe(EventHandler<ResizeEventArgs>? value)
    {
        _logger.LogDebug($"[ViewportResizeListener] Unsubscribe Invoked with value: [{value}]");

        _onResized -= value;
        if (_onResized is null)
            Task.Run(async () => await Cancel().ConfigureAwait(false));
    }

    private async ValueTask<bool> Start()
    {
        _logger.LogDebug("[ViewportResizeListener] Start Invoked");

        // TODO: Remove this later
        _options.EnableLogging = true;

        var module = await _moduleTask.Value;

        try
        {
            await module.InvokeVoidAsync("resizeListener", DotNetObjectReference.Create(this), _options, jsListenerId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[ViewportResizeListener] Start Failed");
            return false;
        }
    }

    private async ValueTask Cancel()
    {
        _logger.LogDebug("[ViewportResizeListener] Cancel Invoked");

        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("cancelListener", jsListenerId);

        // Reset this cache
        _lastBreakpoint = Breakpoint.None;
    }
}
