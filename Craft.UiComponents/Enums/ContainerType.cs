﻿using System.ComponentModel;

namespace Craft.UiComponents.Enums;

public enum ContainerType
{
    [Description("")]
    Default,

    [Description("xs")]
    ExtraSmall,

    [Description("sm")]
    Small,

    [Description("md")]
    Medium,

    [Description("lg")]
    Large,

    [Description("xl")]
    ExtraLarge,

    [Description("xxl")]
    WideScreen,

    [Description("fluid")]
    Fluid
}
