using Craft.MediaQuery.Enums;

namespace Craft.MediaQuery.Models;

public class ResizeEventArgs(ViewportSizeEventArgs viewportSize, Breakpoint breakpoint, bool isFirst = false) : EventArgs
{
    public Breakpoint Breakpoint { get; set; } = breakpoint;
    public bool IsFirst { get; set; } = isFirst;
    public ViewportSizeEventArgs ViewportSize { get; set; } = viewportSize;
}

public class ContainerResizeEventArgs(string elementId,
    ViewportSizeEventArgs viewportSize,
    Breakpoint breakpoint,
    bool isFirst = false) : ResizeEventArgs(viewportSize, breakpoint, isFirst)
{
    public string ElementId { get; set; } = elementId;
}
