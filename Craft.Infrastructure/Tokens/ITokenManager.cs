using System.Security.Claims;
using Craft.Security.AuthModels;
using Craft.Security.Models;

namespace Craft.Infrastructure.Tokens;

public interface ITokenManager
{
    AuthResponse GenerateJwtTokens(IEnumerable<Claim> claims);

    string GenerateTokens(IEnumerable<Claim> claims);

    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);

    RefreshToken GetRefreshToken(CraftUser user);

    bool ValidateToken(string token);
}
