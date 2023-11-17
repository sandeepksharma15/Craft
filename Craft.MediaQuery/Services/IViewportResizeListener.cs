﻿using Craft.MediaQuery.Enums;
using Craft.MediaQuery.Models;

namespace Craft.MediaQuery.Services;

public interface IViewportResizeListener
{
    event EventHandler<ResizeEventArgs> OnResized;

    ValueTask<ViewportSize> GetViewportSize();

    ValueTask<Breakpoint> GetBreakpoint();

    ValueTask<bool> MatchMedia(string mediaQuery);
}
