using Microsoft.AspNetCore.Components;

namespace Craft.Components.Components;

public partial class IconButton
{
    [Parameter] public string Icon { get; set; } = "bi-star"; // Default icon

    [Parameter] public string Size { get; set; } = "40px"; // Button size

    [Parameter] public string IconSize { get; set; } = "1.5rem"; // Icon size

    [Parameter] public string IconColor { get; set; } = "#000"; // Icon color

    [Parameter] public string ButtonClass { get; set; } = ""; // Additional button classes

    [Parameter] public string Style { get; set; } = ""; // Additional inline styles

    [Parameter] public EventCallback OnClick { get; set; }
}
