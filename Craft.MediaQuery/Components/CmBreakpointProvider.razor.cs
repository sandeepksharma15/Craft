using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;
using Craft.MediaQuery.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Craft.MediaQuery.Components;

public partial class CmBreakpointProvider : ComponentBase, IDisposable
{
    [Inject] private IViewportResizeListener _viewportResizeListener { get; set; }
    [Inject] private ILogger<CmBreakpointProvider> _logger { get; set; }

    public Breakpoint Breakpoint { get; private set; } = Breakpoint.Always;

    [Parameter] public EventCallback<Breakpoint> OnBreakpointChanged { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
            _viewportResizeListener.OnResized += WindowResized;
    }

    async void WindowResized(object _, ResizeEventArgs resizeEventArgs)
    {
        _logger.LogDebug("Window is resized Resized");

        Breakpoint = resizeEventArgs.Breakpoint;

        await OnBreakpointChanged.InvokeAsync(resizeEventArgs.Breakpoint);

        StateHasChanged();
    }

    public void Dispose()
    {
        _viewportResizeListener.OnResized -= WindowResized;
    }
}
