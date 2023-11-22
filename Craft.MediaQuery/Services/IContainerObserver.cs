using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;

namespace Craft.MediaQuery.Services;

public interface IContainerObserver
{
    delegate void ResizeEventHandler(object sender, ResizeEventArgs e);

    event EventHandler<ResizeEventArgs> OnResized;

    ValueTask<ViewportSize> GetViewportSize();

    ValueTask<Breakpoint> GetBreakpoint();

    ValueTask<bool> MatchMedia(string mediaQuery);

    ValueTask<bool> IsBreakpointMatching(Breakpoint withBreakpoint);

    bool AreBreakpointsMatching(Breakpoint one, Breakpoint another);

    ValueTask<bool> MatchMediaQuery(int? minWidth = null, int? maxWidth = null);
}
