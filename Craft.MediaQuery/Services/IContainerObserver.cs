using Craft.MediaQuery.Models;

namespace Craft.MediaQuery.Services;

public interface IContainerObserver
{
    ResizeOptions ResizeOptions => null;

    Task NotifyChangeAsync(ResizeEventArgs resizeEventArgs);
}
