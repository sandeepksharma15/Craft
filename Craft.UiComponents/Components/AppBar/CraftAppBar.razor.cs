using Craft.UiComponents.Base;
using Craft.UiComponents.Enums;
using Craft.Utilities.Builders;
using Microsoft.AspNetCore.Components;

namespace Craft.UiComponents.Components.AppBar;

public partial class CraftAppBar : CraftComponent
{
    protected string Classname => new CssBuilder("navbar")
        .AddClass(Expand.GetDescription())
        .AddClass($"bg-{Color.GetDescription()}", Color != BgColor.Default)
        .AddClass($"container-{ContainerType.GetDescription()}")
        .AddClass(Class)
        .Build();

    [Parameter] public BgColor Color { get; set; } = BgColor.Default;

    [Parameter] public AppBarExpand Expand { get; set; } = AppBarExpand.LargeUp;

    [Parameter] public ContainerType ContainerType { get; set; } = ContainerType.Fluid;

}
