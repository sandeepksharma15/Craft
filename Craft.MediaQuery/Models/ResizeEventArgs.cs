using Craft.MediaQuery.Enums;

namespace Craft.MediaQuery.Models;

public class ResizeEventArgs(ViewportSize viewportSize, Breakpoint breakpoint, bool isFirst = false) : EventArgs
{
    public ViewportSize ViewportSize { get; set; } = viewportSize;

    public Breakpoint Breakpoint { get; set; } = breakpoint;

    public bool IsFirst { get; set; } = isFirst;
}
