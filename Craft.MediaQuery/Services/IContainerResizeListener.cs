using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;

namespace Craft.MediaQuery.Services;

public interface IContainerResizeListener
{
    ResizeOptions ResizeOptions { get; }

    Task SubscribeAsync(string elementId, IContainerObserver observer, bool fireImmediately = true);

    Task UnsubscribeAsync(string elementId);

    ValueTask<ViewportSize> GetContainerSizeAsync(string elementId);

    ValueTask<Breakpoint> GetContainerBreakpointAsync(string elementId);

    ValueTask<bool> MatchContainerQueryAsync(string elementId, string containerQuery);

    ValueTask<bool> IsContainerBreakpointMatchingAsync(string elementId, Breakpoint withBreakpoint);

    ValueTask<bool> MatchContainerQueryAsync(string elementId, int? minWidth = null, int? maxWidth = null,
        int? minHeight = null, int? maxHeight = null);
}
