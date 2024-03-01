using Craft.Components.Navigations;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace Craft.Components.UiHelpers;

public partial class StatusMessage
{
    private string message;
    private string? MessageFromCookie => HttpContext.Request.Cookies[RedirectManager.StatusCookieName];

    [CascadingParameter] private HttpContext HttpContext { get; set; } = default!;

    [Parameter] public string? Message { get; set; }

    protected override void OnInitialized()
    {
        message = Message ?? MessageFromCookie;

        if (MessageFromCookie is not null)
            HttpContext.Response.Cookies.Delete(RedirectManager.StatusCookieName);
    }
}
