using Craft.UiComponents.Base;
using Craft.UiComponents.Enums;
using Craft.Utilities.Builders;
using Microsoft.AspNetCore.Components;

namespace Craft.UiComponents.Components.IconButton;

public partial class CraftIconButton : CraftComponent
{
    protected string ClassName => new CssBuilder("icon-button")
        .AddClass($"icon-button-{Color.GetDescription()}", Color != CraftColorScheme.Default)
        .AddClass($"{Size.GetDescription()}")
        .AddClass("text-nowrap", WrapText == false)
        .AddClass(Class)
        .Build();

    [Parameter] public string IconName { get; set; } = "fa-home";

    [Parameter] public CraftSize Size { get; set; } = CraftSize.Default;

    [Parameter] public CraftColorScheme Color { get; set; } = CraftColorScheme.Default;

    [Parameter] public string Text { get; set; } = "Home";
    [Parameter] public string TextClass { get; set; } = "small";

    [Parameter] public bool WrapText { get; set; } = true;

    [Parameter] public EventCallback OnClick { get; set; }

    [Parameter] public bool IsDisabled { get; set; } = false;

    private async Task HandleClick()
    {
        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync();
    }
}
