using Craft.Components.Base;
using Microsoft.AspNetCore.Components;

namespace Craft.Components.UiHelpers;

public partial class DarkModeSwitch : CraftComponent
{
    [Parameter] public bool DarkMode { get; set; }
    [Parameter] public EventCallback<bool> DarkModeChanged { get; set; }

    public void ToggleLightMode(bool changed)
    {
        DarkMode = changed;
        DarkModeChanged.InvokeAsync(DarkMode);
    }
}
