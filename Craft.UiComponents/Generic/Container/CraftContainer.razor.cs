using Craft.UiComponents.Base;
using Craft.UiComponents.Enums;
using Craft.Utilities.Builders;
using Microsoft.AspNetCore.Components;

namespace Craft.UiComponents.Generic.Container;

public partial class CraftContainer : CraftComponent
{
    protected string Classname =>
        new CssBuilder("")
            .AddClass("container", MaxWidth == ContainerSize.Default)
            .AddClass($"container-{MaxWidth.GetDescription()}", MaxWidth != ContainerSize.Default)
            .AddClass(Class)
            .Build();

    [Parameter] public ContainerSize MaxWidth { get; set; } = ContainerSize.Default;
}
