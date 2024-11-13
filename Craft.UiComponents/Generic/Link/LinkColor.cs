using System.ComponentModel;

namespace Craft.UiComponents.Generic.Link;

public enum LinkColor
{
    [Description("")]
    Default,

    [Description("primary")]
    Primary,
    [Description("secondary")]
    Secondary,
    [Description("success")]
    Success,
    [Description("danger")]
    Danger,
    [Description("warning")]
    Warning,
    [Description("info")]
    Info,

    [Description("light")]
    Light,
    [Description("dark")]
    Dark,

    [Description("body-emphasis")]
    Emphasis,
}
