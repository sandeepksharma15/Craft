using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;

namespace Craft.Components.Navigations;

public sealed class RedirectManager(NavigationManager navigationManager, IHttpContextAccessor httpContextAccessor)
{
    private static readonly CookieBuilder _statusCookieBuilder = new()
    {
        SameSite = SameSiteMode.Strict,
        HttpOnly = true,
        IsEssential = true,
        MaxAge = TimeSpan.FromSeconds(5),
    };

    public const string StatusCookieName = "Craft.StatusMessage";

    [DoesNotReturn]
    public void RedirectTo(string uri, bool forceLoad = false)
    {
        if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
            uri = navigationManager.ToBaseRelativePath(uri);

        // This works because either:
        // [1] NavigateTo() throws NavigationException, which is handled by the framework as a redirect.
        // [2] NavigateTo() throws some other exception, which gets treated as a normal unhandled exception.
        // [3] NavigateTo() does not throw an exception, meaning we're not rendering from an endpoint, so we throw
        //     an InvalidOperationException to indicate that we can't redirect.
        navigationManager.NavigateTo(uri, forceLoad);
        // throw new InvalidOperationException("Can only redirect when rendering from an endpoint");
    }

    [DoesNotReturn]
    public void RedirectTo(string uri, Dictionary<string, object> queryParameters, bool forceLoad = false)
    {
        var uriWithoutQuery = navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
        var newUri = navigationManager.GetUriWithQueryParameters(uriWithoutQuery, queryParameters);

        RedirectTo(newUri, forceLoad);
    }

    [DoesNotReturn]
    public void RedirectToCurrentPage(bool forceLoad = false)
        => RedirectTo(navigationManager.Uri, forceLoad);

    [DoesNotReturn]
    public void RedirectToCurrentPageWithStatus(string message, bool forceLoad = false)
        => RedirectToWithStatus(navigationManager.Uri, message, forceLoad);

    [DoesNotReturn]
    public void RedirectToLogin(string token, bool persist, string returnUrl = "/")
    {
        // Create A Dictionary of Query Parameters
        var queryParameters = new Dictionary<string, object>
        {
            { "paramToken", token },
            { "paramPersistent", persist },
            { "paramReturnUrl", returnUrl }
        };

        RedirectTo("/login", queryParameters, true);
    }

    [DoesNotReturn]
    public void RedirectToWithStatus(string uri, string message, bool forceLoad = false)
    {
        var httpContext = httpContextAccessor.HttpContext ??
            throw new InvalidOperationException($"{nameof(RedirectToWithStatus)} requires access to an {nameof(HttpContext)}.");
        httpContext.Response.Cookies.Append(StatusCookieName, message, _statusCookieBuilder.Build(httpContext));

        RedirectTo(uri, forceLoad);
    }
}
