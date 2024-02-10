using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;

namespace Craft.MediaQuery.Services;

public interface IViewportResizeListener
{
    event EventHandler<ResizeEventArgs> OnResized;

    ValueTask<Breakpoint> GetBreakpointAsync();

    ValueTask<ViewportSizeEventArgs> GetViewportSizeAsync();

    ValueTask<bool> IsBreakpointMatchingAsync(Breakpoint withBreakpoint);

    ValueTask<bool> MatchMediaAsync(string mediaQuery);

    ValueTask<bool> MatchMediaAsync(int? minWidth = null, int? maxWidth = null);
}
