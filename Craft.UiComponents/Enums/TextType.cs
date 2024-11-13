using System.ComponentModel;

namespace Craft.UiComponents.Enums;

public enum TextType
{
    [Description("")]
    Default,

    [Description("h1")]
    H1,
    [Description("h2")]
    H2,
    [Description("h3")]
    H3,
    [Description("h4")]
    H4,
    [Description("h5")]
    H5,
    [Description("h6")]
    H6,

    [Description("fs-1")]
    FS1,
    [Description("fs-2")]
    FS2,
    [Description("fs-3")]
    FS3,
    [Description("fs-4")]
    FS4,
    [Description("fs-5")]
    FS5,
    [Description("fs-6")]
    FS6,

    [Description("display-1")]
    Display1,
    [Description("display-2")]
    Display2,
    [Description("display-3")]
    Display3,
    [Description("display-4")]
    Display4,
    [Description("display-5")]
    Display5,
    [Description("display-6")]
    Display6,

    [Description("mark")]
    Mark,
    [Description("small")]
    Small,
    [Description("lead")]
    Lead,
    [Description("text-decoration-underline")]
    Underline,
    [Description("text-decoration-line-through")]
    LineThrough,

    [Description("initialism")]
    AbbrInitialism,

    [Description("blockquote")]
    Blockquote,
    [Description("blockquote-footer")]
    Blockquote_Footer,
}
