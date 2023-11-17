using Craft.MediaQuery.Models;

namespace Craft.MediaQuery.Services;

public interface IViewportResizeListener
{
    event EventHandler<ViewportSize> OnResized;

    ValueTask<ViewportSize> GetViewportSize();

    ValueTask<bool> MatchMedia(string mediaQuery);
}
