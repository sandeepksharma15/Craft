using System.Text.Json;
using System.Text.Json.Serialization;

namespace Craft.Security.AuthModels;

public record AuthResponse([property: JsonPropertyName("jwtToken")] string JwtToken,
    [property: JsonPropertyName("refreshToken")] string RefreshToken,
    [property: JsonPropertyName("refreshTokenExpiryTime")] DateTime RefreshTokenExpiryTime)
{
    public static AuthResponse Empty
        => new(string.Empty, string.Empty, DateTime.MinValue);

    public bool IsEmpty
        => JwtToken?.Length == 0 && RefreshToken?.Length == 0 && RefreshTokenExpiryTime == DateTime.MinValue;

    public static AuthResponse GetAuthResult(string jsonData)
    {
        if (jsonData.IsNullOrEmpty())
            return Empty;

        var authResult = JsonSerializer.Deserialize<AuthResponse>(jsonData);

        return authResult ?? Empty;
    }

    public static AuthResponse GetAuthResult(object jsonData)
    {
        if (jsonData == null)
            return Empty;

        return GetAuthResult(jsonData.ToString());
    }

    public RefreshTokenRequest ToRefreshTokenRequest(string ipAddress)
        => new(JwtToken, RefreshToken, ipAddress);
}
