#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Components;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class NavigationExtensions
{
    public static string GetAbsoluteUriWithoutQueryParameters(this NavigationManager navigationManager)
    {
        var uri = navigationManager.ToAbsoluteUri(navigationManager.Uri);
        return uri.GetLeftPart(UriPartial.Path);
    }

    public static string GetBaseRelativePathWithoutQueryParameters(this NavigationManager navigationManager)
    {
        var uri = navigationManager.ToBaseRelativePath(navigationManager.Uri);

        return uri.Split('?')[0];
    }
}
