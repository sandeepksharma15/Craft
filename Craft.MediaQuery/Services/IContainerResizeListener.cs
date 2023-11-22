using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;

namespace Craft.MediaQuery.Services;

public interface IContainerResizeListener
{
    ResizeOptions ResizeOptions { get; }

    Task SubscribeAsync(string elementId, IContainerObserver observer, bool fireImmediately = true);

    Task UnsubscribeAsync(IContainerObserver observer);

    ValueTask<ViewportSize> GetContainerSizeAsync();

    ValueTask<Breakpoint> GetContainerBreakpointAsync();

    ValueTask<bool> MatchContainerQuery(string mediaQuery);

    ValueTask<bool> IsContainerBreakpointMatching(Breakpoint withBreakpoint);

    ValueTask<bool> MatchContainerQuery(int? minWidth = null, int? maxWidth = null);
}
