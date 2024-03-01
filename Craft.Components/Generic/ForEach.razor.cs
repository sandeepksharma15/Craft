using Craft.Components.Base;
using Microsoft.AspNetCore.Components;

namespace Craft.Components.Generic;

public partial class ForEach<T> : CraftComponent
{
    [Parameter]
    public new RenderFragment<T>? ChildContent { get; set; }

    [Parameter]
    [EditorRequired]
    public IEnumerable<T>? Collection { get; set; }
}
