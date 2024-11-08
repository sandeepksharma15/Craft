using Craft.UiComponents.Base;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Craft.Components.Table;

public partial class SortIcon : CraftComponent
{
    private string icon;

    [Parameter] public Craft.Components.Table.Enums.SortDirection SortDirection { get; set; }
    [Parameter] public string UpIcon { get; set; } = Icons.Material.Outlined.ArrowUpward;
    [Parameter] public string DownIcon { get; set; } = Icons.Material.Outlined.ArrowDownward;

    protected override void OnParametersSet()
    {
        base.OnParametersSet();

        icon = SortDirection == Craft.Components.Table.Enums.SortDirection.Descending ? UpIcon : DownIcon;
    }
}
