using Microsoft.AspNetCore.Components;

namespace Craft.Components.UiBuilder;

public partial class UserCard
{
    [Parameter] public string Class { get; set; }
    [Parameter] public string ImageUrl { get; set; }
    [Parameter] public string Name { get; set; }
    [Parameter] public string Role { get; set; }
    [Parameter] public string Style { get; set; }
}
