using Craft.UiComponents.Base;
using Craft.UiComponents.Enums;
using Craft.Utilities.Builders;
using Microsoft.AspNetCore.Components;

namespace Craft.UiComponents.Generic.Container;

public partial class CraftContainer : CraftComponent
{
    protected string Classname =>
        new CssBuilder("")
            .AddClass("container", MaxWidth == ContainerType.Default)
            .AddClass($"container-{MaxWidth.GetDescription()}", MaxWidth != ContainerType.Default)
            .AddClass(Class)
            .Build();

    [Parameter] public ContainerType MaxWidth { get; set; } = ContainerType.Default;
}
