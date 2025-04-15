using Microsoft.AspNetCore.Components;

namespace Craft.UiComponents.Components;

public partial class ShowProcessing
{
    [Parameter] public bool IsProcessing { get; set; }

    public void StartProcessing()
    {
        IsProcessing = true;
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
