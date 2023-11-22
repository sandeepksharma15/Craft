using Craft.MediaQuery.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace Craft.MediaQuery.Services;

public class ContainerObserver : IAsyncDisposable
{
    private readonly ILogger<ViewportResizeListener> _logger;
    private readonly ResizeOptions _options;
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public ContainerObserver(IJSRuntime jsRuntime,
        ILogger<ViewportResizeListener> logger,
        IOptions<ResizeOptions>? options = null)
    {
        _logger = logger;

        _moduleTask = new(()
            => jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Craft.MediaQuery/containerObserver.js").AsTask());

        // Get the user options and set Defaults if not provided
        _options = options?.Value ?? GlobalOptions.GetDefaultResizeOptions();
        _options.Breakpoints = GlobalOptions.GetBreakpoints(_options);
    }

    public ValueTask DisposeAsync()
    {
        throw new NotImplementedException();
    }
}
