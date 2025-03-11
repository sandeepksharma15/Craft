using Craft.Utilities.Helpers;
using Microsoft.AspNetCore.Components;

namespace Craft.UiComponents.Components.RunningNumber;

public partial class CraftRunningNumber : ComponentBase, IDisposable
{
    [Parameter] public int Timeout { get; set; } = 1;
    [Parameter] public long StartAt { get; set; } = 0;

    [Parameter] public long EndAt { get; set; } = 100;

    private CountdownTimer? _countdownTimer;
    private long _number = 0;
    private float _factor;
    private bool _disposed;

    protected override void OnParametersSet()
    {
        _countdownTimer = new CountdownTimer(Timeout);
        _countdownTimer.OnTick += IncrementNumber;
        _countdownTimer.Start();

        _factor = ((float)(EndAt - StartAt)) / 100;
    }

    private async void IncrementNumber(int percentComplete)
    {
        _number = StartAt + ((long)Math.Ceiling((_factor * percentComplete)));

        await InvokeAsync(StateHasChanged).ConfigureAwait(false);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _countdownTimer?.Dispose();
            _countdownTimer = null;
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
