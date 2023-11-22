using System.Runtime.CompilerServices;

namespace Craft.MediaQuery.Enums;

public enum Breakpoint
{
    ExtraSmall,
    Mobile,
    Tablet,
    Desktop,
    Widescreen,
    FullHd,

    MobileAndDown,
    TabletAndDown,
    DesktopAndDown,
    WidescreenAndDown,

    MobileAndUp,
    TabletAndUp,
    DesktopAndUp,
    WidescreenAndUp,

    None,
    Always
}

public static class BreakpointExtensions
{
    public static bool IsMatchingWith(this Breakpoint one, Breakpoint another)
    {
        return another switch
        {
            Breakpoint.ExtraSmall => one == Breakpoint.ExtraSmall,
            Breakpoint.Mobile => one == Breakpoint.Mobile,
            Breakpoint.Tablet => one == Breakpoint.Tablet,
            Breakpoint.Desktop => one == Breakpoint.Desktop,
            Breakpoint.Widescreen => one == Breakpoint.Widescreen,
            Breakpoint.FullHd => one == Breakpoint.FullHd,

            Breakpoint.MobileAndDown => one <= Breakpoint.Mobile,
            Breakpoint.TabletAndDown => one <= Breakpoint.Tablet,
            Breakpoint.DesktopAndDown => one <= Breakpoint.Desktop,
            Breakpoint.WidescreenAndDown => one <= Breakpoint.Widescreen,

            Breakpoint.MobileAndUp => one >= Breakpoint.Mobile,
            Breakpoint.TabletAndUp => one >= Breakpoint.Tablet,
            Breakpoint.DesktopAndUp => one >= Breakpoint.Desktop,
            Breakpoint.WidescreenAndUp => one >= Breakpoint.Widescreen,

            Breakpoint.None => false,
            Breakpoint.Always => true,

            _ => false
        };
    }
}
