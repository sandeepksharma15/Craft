using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;

namespace Craft.MediaQuery.Services;

public interface IContainerResizeListener
{
    ResizeOptions ResizeOptions { get; }

    ValueTask<Breakpoint> GetContainerBreakpointAsync(string elementId);

    ValueTask<ViewportSizeEventArgs> GetContainerSizeAsync(string elementId);

    ValueTask<bool> IsContainerBreakpointMatchingAsync(string elementId, Breakpoint withBreakpoint);

    Task SubscribeAsync(string elementId, IContainerObserver observer, bool fireImmediately = true);

    Task UnsubscribeAsync(string elementId);
}
