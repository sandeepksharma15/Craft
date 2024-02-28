using System.Security.Claims;

namespace Craft.Security.CurrentUserService;

public interface ICurrentUserProvider
{
    ClaimsPrincipal GetUser();
}
