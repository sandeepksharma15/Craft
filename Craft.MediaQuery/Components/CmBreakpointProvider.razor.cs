using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;
using Craft.MediaQuery.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Craft.MediaQuery.Components;

public partial class CmBreakpointProvider : ComponentBase, IDisposable
{
    public Breakpoint Breakpoint { get; private set; } = Breakpoint.None;
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<Breakpoint> OnBreakpointChanged { get; set; }
    [Inject] internal ILogger<CmBreakpointProvider> _logger { get; set; }
    [Inject] internal IViewportResizeListener _viewportResizeListener { get; set; }

    public void Dispose()
    {
        _viewportResizeListener.OnResized -= WindowResized;

        GC.SuppressFinalize(this);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
            _viewportResizeListener.OnResized += WindowResized;
    }

    private async void WindowResized(object _, ResizeEventArgs resizeEventArgs)
    {
        _logger.LogDebug("Window is resized Resized");

        Breakpoint = resizeEventArgs.Breakpoint;

        await OnBreakpointChanged.InvokeAsync(resizeEventArgs.Breakpoint);

        StateHasChanged();
    }
}
