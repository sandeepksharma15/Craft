using Craft.Components.Base;
using Microsoft.AspNetCore.Components;

namespace Craft.Components.Generic;

public partial class Conditional : CraftComponent
{
    [Parameter]
    [EditorRequired]
    public bool Condition { get; set; }

    [Parameter]
    public RenderFragment? False { get; set; }

    [Parameter]
    public RenderFragment? True { get; set; }
}
