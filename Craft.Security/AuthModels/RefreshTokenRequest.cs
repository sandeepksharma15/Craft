namespace Craft.Security.AuthModels;

public record RefreshTokenRequest(string JwtToken,
    string RefreshToken,
    string ipAddress);
