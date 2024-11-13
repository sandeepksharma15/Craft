using Craft.UiComponents.Base;
using Craft.UiComponents.Enums;
using Craft.Utilities.Builders;
using Microsoft.AspNetCore.Components;

namespace Craft.UiComponents.Generic.Link;

public partial class CraftLink : CraftComponent
{
    protected string Classname =>
            new CssBuilder("")
                .AddClass($"link-{Color.GetDescription()}", Color != LinkColor.Default)
                .AddClass($"link-underline-{UnderLineColor.GetDescription()}", Color != LinkColor.Default)

                .AddClass($"link-{Opacity.GetDescription()}", Opacity != Opacity.Default)
                .AddClass($"link-underline link-underline-{UnderlineOpacity.GetDescription()}", Opacity != Opacity.Default)
                .AddClass($"link-{HoverOpacity.GetDescription()}-hover", Opacity != Opacity.Default)

                .AddClass($"link-{Offset.GetDescription()}", Offset != LinkOffset.Default)

                .AddClass($"{TextType.GetDescription()}", TextType != TextType.Default)

                .AddClass("IsDisabled", Disabled)
                .AddClass("stretched-link", Stretched)
                .AddClass(Class)
            .Build();

    [Parameter] public LinkColor Color { get; set; } = LinkColor.Default;
    [Parameter] public LinkColor UnderLineColor { get; set; } = LinkColor.Default;

    [Parameter] public Opacity Opacity { get; set; } = Opacity.Default;
    [Parameter] public Opacity UnderlineOpacity { get; set; } = Opacity.Default;
    [Parameter] public Opacity HoverOpacity { get; set; } = Opacity.Default;

    [Parameter] public LinkOffset Offset { get; set; } = LinkOffset.Default;

    [Parameter] public TextType TextType { get; set; } = TextType.Default;

    [Parameter] public string Href { get; set; } = string.Empty;
    [Parameter] public string Target { get; set; } = string.Empty;

    [Parameter] public bool Disabled { get; set; }
    [Parameter] public bool Stretched { get; set; }
}
