using Craft.MediaQuery.Models;

namespace Craft.MediaQuery.Services;

public interface IContainerObserver
{
    Guid Id { get; }

    ResizeOptions ResizeOptions => null;

    Task NotifyChangeAsync(ResizeEventArgs resizeEventArgs);
}
