using Craft.UiComponents.Base;
using Craft.UiComponents.Enums;
using Craft.Utilities.Builders;
using Microsoft.AspNetCore.Components;

namespace Craft.UiComponents.Generic.Container;

public partial class CraftContainer : CraftComponent
{
    protected string Classname =>
        new CssBuilder("")
            .AddClass("container", (MaxWidth == ViewPortSize.Default && IsFluid != true))
            .AddClass($"container-{MaxWidth.GetDescription()}", (MaxWidth != ViewPortSize.Default && IsFluid != true))
            .AddClass($"container-fluid", IsFluid == true)
            .AddClass(Class)
            .Build();

    [Parameter] public ViewPortSize MaxWidth { get; set; } = ViewPortSize.Default;

    [Parameter] public bool IsFluid { get; set; } 
}
