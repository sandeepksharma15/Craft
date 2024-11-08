using Craft.UiComponents.Base;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace Craft.Components.UiBuilder;

public partial class PageHeader : CraftComponent
{
    private const string _captionClass = "mud-typography mud-typography-caption pa-0";

    [Parameter] public bool ExportIcon { get; set; }
    [Parameter] public List<BreadcrumbItem> Items { get; set; } = [];
    [Parameter] public EventCallback OnExportClick { get; set; }
    [Parameter] public EventCallback OnSearchClick { get; set; }
    [Parameter] public bool SearchIcon { get; set; }
    [Parameter] public string Separator { get; set; } = ">";
    [Parameter] public bool ShareIcon { get; set; }
    [Parameter] public string Title { get; set; } = "Page Title";

    private static void OnShareClick()
    { }

    public void SetOnExportClickCallback(EventCallback eventCallback) => OnExportClick = eventCallback;

    public void SetOnSearchClickCallback(EventCallback eventCallback) => OnSearchClick = eventCallback;

    public void ShowExportIcon(bool show) => ExportIcon = show;

    public void ShowSearchIcon(bool show) => SearchIcon = show;

    public void ShowShareIcon(bool show) => ShareIcon = show;
}
