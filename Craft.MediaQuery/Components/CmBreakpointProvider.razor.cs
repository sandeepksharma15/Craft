using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;
using Craft.MediaQuery.Services;

using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Craft.MediaQuery.Components;

public partial class CmBreakpointProvider : ComponentBase, IDisposable
{
    #region Internal Properties

    [Inject] internal ILogger<CmBreakpointProvider> _logger { get; set; }
    [Inject] internal IViewportResizeListener _viewportResizeListener { get; set; }

    #endregion Internal Properties

    #region Public Properties

    public Breakpoint Breakpoint { get; private set; } = Breakpoint.None;

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public EventCallback<Breakpoint> OnBreakpointChanged { get; set; }

    #endregion Public Properties

    #region Private Methods

    private async void WindowResized(object _, ResizeEventArgs resizeEventArgs)
    {
        _logger.LogDebug("Window is resized Resized");

        Breakpoint = resizeEventArgs.Breakpoint;

        await OnBreakpointChanged.InvokeAsync(resizeEventArgs.Breakpoint);

        StateHasChanged();
    }

    #endregion Private Methods

    #region Protected Methods

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
            _viewportResizeListener.OnResized += WindowResized;
    }

    #endregion Protected Methods

    #region Public Methods

    public void Dispose()
    {
        _viewportResizeListener.OnResized -= WindowResized;

        GC.SuppressFinalize(this);
    }

    #endregion Public Methods
}