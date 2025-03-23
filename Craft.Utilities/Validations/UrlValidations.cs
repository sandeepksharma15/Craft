namespace Craft.Utilities.Validations;

public static class UrlValidations
{
    public static bool IsValidUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return false;

        return Uri.TryCreate(url, UriKind.Absolute, out Uri? uriResult)
            && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
    }

    public static async Task<bool> IsUrlReachableAsync(string url)
    {
        try
        {
            using HttpClient client = new();

            client.Timeout = TimeSpan.FromSeconds(5);

            var response = await client.SendAsync(
                new HttpRequestMessage(HttpMethod.Head, url));

            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }
}
