using Craft.UiComponents.Base;
using Craft.UiComponents.Enums;
using Craft.Utilities.Builders;
using Microsoft.AspNetCore.Components;

namespace Craft.UiComponents.Components.CoolButton;

public partial class CraftCoolButton : CraftComponent
{
    protected string ClassName => new CssBuilder("btn btn-cool")
        .AddClass($"btn-{Color.GetDescription()}", Color != CraftColorScheme.Default)
        .AddClass($"{Size.GetDescription()}")
        .AddClass(Class)
        .Build();

    [Parameter] public string IconName { get; set; } = "fas fa-home";

    [Parameter] public CraftSize Size { get; set; } = CraftSize.Default;

    [Parameter] public CraftColorScheme Color { get; set; } = CraftColorScheme.Default;

    [Parameter] public string Text { get; set; } = "Home";

    [Parameter] public EventCallback OnClick { get; set; }

    [Parameter] public bool IsDisabled { get; set; } = false;

    private async Task HandleClick()
    {
        if (OnClick.HasDelegate)
            await OnClick.InvokeAsync();
    }
}
