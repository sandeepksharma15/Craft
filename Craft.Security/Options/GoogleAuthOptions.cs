using System.ComponentModel.DataAnnotations;

namespace Craft.Security.Options;

public class GoogleAuthOptions : IValidatableObject
{
    public const string SectionName = "Authentication:Google";

    public string ClientId { get; set; }
    public string ProjectId { get; set; } = string.Empty;
    public string AuthUri { get; set; } = string.Empty;
    public string TokenUri { get; set; } = string.Empty;
    public string AuthProviderX509CertUrl { get; set; } = string.Empty;
    public string ClientSecret { get; set; }
    public string[] RedirectUris { get; set; } = [];
    public string[] JavascriptOrigins { get; set; } = [];

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (ClientId.IsNullOrEmpty())
            yield return new ValidationResult(
                $"{nameof(GoogleAuthOptions)}.{nameof(ClientId)} is not configured",
                [nameof(ClientId)]);

        if (ClientSecret.IsNullOrEmpty())
            yield return new ValidationResult(
                $"{nameof(GoogleAuthOptions)}.{nameof(ClientSecret)} is not configured",
                [nameof(ClientSecret)]);

        if (RedirectUris.IsNullOrEmpty())
            yield return new ValidationResult(
                $"{nameof(GoogleAuthOptions)}.{nameof(RedirectUris)} is not configured",
                [nameof(RedirectUris)]);
    }
}
