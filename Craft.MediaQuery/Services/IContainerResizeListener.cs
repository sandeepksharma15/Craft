using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;

namespace Craft.MediaQuery.Services;

public interface IContainerResizeListener
{
    ResizeOptions ResizeOptions { get; }

    Task SubscribeAsync(string elementId, IContainerObserver observer, bool fireImmediately = true);

    Task UnsubscribeAsync(Guid observerId);

    ValueTask<ViewportSize> GetContainerSizeAsync(Guid observerId);

    ValueTask<Breakpoint> GetContainerBreakpointAsync(Guid observerId);

    ValueTask<bool> MatchContainerQueryAsync(Guid observerId, string containerQuery);

    ValueTask<bool> IsContainerBreakpointMatchingAsync(Guid observerId, Breakpoint withBreakpoint);

    ValueTask<bool> MatchContainerQueryAsync(Guid observerId, int? minWidth = null, int? maxWidth = null);
}
