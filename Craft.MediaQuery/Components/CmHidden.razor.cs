using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;
using Craft.MediaQuery.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Craft.MediaQuery.Components;

public partial class CmHidden : ComponentBase, IDisposable
{
    private bool _isHidden = true;
    private bool _isServiceReady = false;
    private bool _isServiceSubscribed = false;
    private Breakpoint _currentBreakpoint = Breakpoint.None;

    [Inject] private IViewportResizeListener _viewportResizeListener { get; set; }
    [Inject] private ILogger<CmHidden> _logger { get; set; }

    [CascadingParameter]
    public Breakpoint CurrentBreakpointFromProvider { get; set; } = Breakpoint.None;

    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public Breakpoint Breakpoint { get; set; }
    [Parameter] public bool Invert { get; set; }
    [Parameter] public EventCallback<bool> IsHiddenChanged { get; set; }

    [Parameter]
    public bool IsHidden
    {
        get => _isHidden;
        set
        {
            if (_isHidden != value)
            {
                _isHidden = value;
                IsHiddenChanged.InvokeAsync(value);
            }
        }
    }

    protected override async Task OnParametersSetAsync()
    {
        await base.OnParametersSetAsync();
        await UpdateAsync(_currentBreakpoint);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);

        if (firstRender)
        {
            _isServiceReady = true;

            if (CurrentBreakpointFromProvider == Breakpoint.None)
            {
                _isServiceSubscribed = true;
                _viewportResizeListener.OnResized += WindowResized;
            }
        }
    }

    async void WindowResized(object _, ResizeEventArgs resizeEventArgs)
    {
        _logger.LogDebug("Window Resized");

        await UpdateAsync(resizeEventArgs.Breakpoint);

        StateHasChanged();
    }

    public void Dispose()
    {
        if (_isServiceSubscribed && _isServiceReady)
            _viewportResizeListener.OnResized -= WindowResized;
    }

    protected async Task UpdateAsync(Breakpoint currentBreakpoint)
    {
        if (CurrentBreakpointFromProvider != Breakpoint.None)
            currentBreakpoint = CurrentBreakpointFromProvider;
        else
            if (!_isServiceReady)
                return;

        if (currentBreakpoint == Breakpoint.None)
            return;

        _currentBreakpoint = currentBreakpoint;

        _logger.LogDebug($"Checking {currentBreakpoint} with {Breakpoint}");

        var hidden = currentBreakpoint.IsMatchingWith(Breakpoint);

        if (Invert)
            hidden = !hidden;

        IsHidden = hidden;

        await Task.CompletedTask;
    }
}
