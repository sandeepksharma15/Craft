using System.ComponentModel;

namespace Craft.UiComponents.Enums;

public enum BgColor
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

    [Description("white")]
    White,
    [Description("black")]
    Black,

    [Description("body")]
    Body,
    [Description("transparent")]
    Transparent,
    [Description("body-secondary")]
    BodySecondary,
    [Description("body-tertiary")]
    BodyTertiary,
}
