using Craft.UiComponents.Base;
using Craft.UiComponents.Enums;
using Craft.UiComponents.Generic.Link;
using Craft.Utilities.Builders;
using Microsoft.AspNetCore.Components;

namespace Craft.UiComponents.Components.AppBar;

public partial class AppBrand : CraftComponent
{
    protected string Classname =>
        new CssBuilder("navbar-brand")
            .AddClass(Class)
        .Build();


    [Parameter] public string Href { get; set; } = "/";
    [Parameter] public string Target { get; set; } = string.Empty;

}
