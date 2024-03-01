using Microsoft.AspNetCore.Components;

namespace Craft.Components.Base;

public abstract class CraftComponent : ComponentBase
{
    [Parameter] public RenderFragment ChildContent { get; set; }
    [Parameter] public string Class { get; set; }
    [Parameter] public ElementReference ElementRef { get; set; }
    [Parameter] public Action<ElementReference> ElementRefChanged { get; set; }
    public string Id { get; set; }
    [Parameter] public string Style { get; set; }
    [Parameter] public object Tag { get; set; }
    [Parameter(CaptureUnmatchedValues = true)] public IDictionary<string, object> UserAttributes { get; set; }
    [Parameter] public virtual bool Visible { get; set; } = true;

    protected override void OnInitialized()
    {
        Id = Guid.NewGuid().ToString("N")[..10];
    }

    protected internal string GetId()
    {
        return UserAttributes != null && UserAttributes.TryGetValue("id", out object id)
                && !Convert.ToString(id).IsNullOrEmpty()
            ? Convert.ToString(id)
            : Id;
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        parameters.TryGetValue(nameof(Visible), out bool visibleState);

        await base.SetParametersAsync(parameters);

        if (Visible != visibleState)
            StateHasChanged();
    }
}
