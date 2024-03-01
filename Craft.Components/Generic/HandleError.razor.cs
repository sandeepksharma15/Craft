using System.Diagnostics;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;

namespace Craft.Components.Generic;

public partial class HandleError : ComponentBase
{
    [Inject] protected internal ILogger<HandleError> Logger { get; set; }
    [Inject] protected internal NavigationManager NavigationManager { get; set; }

    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public string ErrorPageUri { get; set; }

    public void ProcessError(Exception ex, string Message = null)
    {
        ArgumentNullException.ThrowIfNull(ex, nameof(ex));

        var errorId = Activity.Current?.Id;

        Logger?.LogError($"Error: ProcessError - Type: {ex.GetType()} Error Id: {errorId} Message: {ex.Message}");

        Message ??= ex.Message;

        Message = Message + " Error Id: [" + errorId + "]";
        NavigationManager?.NavigateTo($"{ErrorPageUri}/{Uri.EscapeDataString(Message)}", true);
    }
}
