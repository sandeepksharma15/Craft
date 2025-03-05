using Microsoft.AspNetCore.Components;

namespace Craft.Components.UiHelpers;

public partial class UnderMaintenance
{
    [Parameter] public DateTime TargetDate { get; set; }

    private TimeSpan _timeRemaining;
    private Timer _timer;

    protected override void OnInitialized()
    {
        UpdateTime();
        _timer = new Timer(UpdateTime, null, 1000, 1000);
    }

    private async void UpdateTime(object state = null)
    {
        var now = DateTime.UtcNow;

        _timeRemaining = TargetDate - now;

        if (_timeRemaining.TotalSeconds <= 0)
        {
            _timeRemaining = TimeSpan.Zero;
            _timer?.Dispose();
        }

        await InvokeAsync(StateHasChanged);
    }
}
