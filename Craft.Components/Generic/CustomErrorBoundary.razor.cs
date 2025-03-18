using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Extensions.Logging;

namespace Craft.Components.Generic;

public partial class CustomErrorBoundary : ErrorBoundary
{
    [Inject] private NavigationManager NavigationManager { get; set; }
    [Inject] private ILogger<CustomErrorBoundary> Logger { get; set; }

    [Parameter] public string ErrorPageUri { get; set; }

    protected override async Task OnErrorAsync(Exception exception)
    {
        Logger.LogError(exception, "An error occurred in the error boundary");
        NavigationManager?.NavigateTo($"{ErrorPageUri}/{Uri.EscapeDataString(exception.Message)}", true);

        await Task.CompletedTask;
    }
}
