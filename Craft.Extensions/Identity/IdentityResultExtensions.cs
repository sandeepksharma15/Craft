#pragma warning disable IDE0130 // Namespace does not match folder structure
namespace Microsoft.AspNetCore.Identity;
#pragma warning restore IDE0130 // Namespace does not match folder structure

public static class IdentityResultExtensions
{
    /// <summary>
    /// Extension method for <see cref="IdentityResult"/> that retrieves a list of error descriptions.
    /// </summary>
    /// <param name="result">The <see cref="IdentityResult"/> instance.</param>
    /// <returns>A list of error descriptions.</returns>
    public static List<string> GetErrors(this IdentityResult result) =>
        [.. result.Errors.Select(e => e.Description)];
}
