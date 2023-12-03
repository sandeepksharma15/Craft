namespace Microsoft.AspNetCore.Identity;

public static class IdentityResultExtensions
{
    /// <summary>
    /// Extension method for <see cref="IdentityResult"/> that retrieves a list of error descriptions.
    /// </summary>
    /// <param name="result">The <see cref="IdentityResult"/> instance.</param>
    /// <returns>A list of error descriptions.</returns>
    public static List<string> GetErrors(this IdentityResult result) =>
        result.Errors.Select(e => e.Description).ToList();
}
