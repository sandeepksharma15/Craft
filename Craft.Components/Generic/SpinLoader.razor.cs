using Craft.UiComponents.Base;
using Microsoft.AspNetCore.Components;

namespace Craft.Components.Generic;

public partial class SpinLoader : CraftComponent
{
    [Parameter] public RenderFragment ContentTemplate { get; set; }

    [Parameter] public bool IsFaulted { get; set; }

    [Parameter]
    [EditorRequired]
    public bool IsLoading { get; set; }

    [Parameter] public RenderFragment LoadingTemplate { get; set; }
}
