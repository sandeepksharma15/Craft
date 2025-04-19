using Craft.UiComponents.Components.Processing;
using Microsoft.AspNetCore.Components;

namespace Craft.UiComponents.Components;

public partial class ShowProcessing
{
    [Parameter] public bool IsProcessing { get; set; }
    [Parameter] public List<ProcessingItem> Steps { get; set; } = 
        [new() { Message="Our AI is working its magic! Magix takes time!!", DurationMs=2000 }];

    private string _currentMessage { get; set; } = "";

    public void StartProcessing()
    {
        // Let The UI Know we are processing
        IsProcessing = true;

        // Update UI 
        StateHasChanged();
    }

    public async Task Start()
    {
        // Let The UI Know we are processing
        IsProcessing = true;

        for (int i = 0; i < Steps.Count; i++)
        {
            if (IsProcessing == false) break;

            _currentMessage = Steps[i].Message;
            StateHasChanged();

            try
            {
                await Task.Delay(Steps[i].DurationMs);
            }
            catch { return; } // In case cancellation is forced
        }

        // All steps shown – now hold the last message until Stop() is called
        StateHasChanged();
    }

    public void Stop()
    {
        IsProcessing = false;
        StateHasChanged();
    }

    public void StopProcessing()
    {
        IsProcessing = false;
        StateHasChanged();
    }

    public void ToggleProcessing()
    {
        IsProcessing = !IsProcessing;
        StateHasChanged();
    }

    public void SetProcessing(bool isProcessing)
    {
        IsProcessing = isProcessing;
        StateHasChanged();
    }
}
