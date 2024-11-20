using System.ComponentModel;

namespace Craft.UiComponents.Components.AppBar;

public enum AppBarExpand
{
    [Description("navbar-expand")]
    Always,

    [Description("navbar-expand-sm")]
    SmallUp,

    [Description("navbar-expand-md")]
    MediumUp,

    [Description("navbar-expand-lg")]
    LargeUp,

    [Description("navbar-expand-xl")]
    ExtraLargeUp,

    [Description("navbar-expand-xxl")]
    WideScreenUp,

    [Description("")]
    Never
}
