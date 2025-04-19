namespace Craft.UiComponents.Components.Processing;

public class ProcessingItem
{
    public string Message { get; set; } = string.Empty;
    public int DurationMs { get; set; } // Time to stay on this step (in ms)


    public bool IsCompleted { get; set; } = false;  // May not be used presently
}
