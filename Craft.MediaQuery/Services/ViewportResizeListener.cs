
using Craft.MediaQuery.Models;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Craft.MediaQuery.Services;

public class ViewportResizeListener(IJSRuntime jsRuntime, IOptions<ResizeOptions>? options = null)
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
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<ViewportSize>("getViewportSize");
    }

    public async ValueTask<bool> MatchMedia(string mediaQuery)
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<bool>("matchMedia", mediaQuery);
    }

    [JSInvokable]
    public void RaiseOnResized(ViewportSize browserWindowSize)
        => _onResized?.Invoke(this, browserWindowSize);

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
                _onResized = null;

            _disposed = true;
        }
    }

    public async ValueTask DisposeAsync()
    {
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
        if (_onResized is null)
            Task.Run(async () => await Start().ConfigureAwait(false));

        _onResized += value;
    }

    private void Unsubscribe(EventHandler<ViewportSize>? value)
    {
        _onResized -= value;
        if (_onResized is null)
            Task.Run(async () => await Cancel().ConfigureAwait(false));
    }

    private async ValueTask<bool> Start()
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<bool>("listenForResize", DotNetObjectReference.Create(this), _options);
    }

    private async ValueTask Cancel()
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("cancelListener");
    }
}
