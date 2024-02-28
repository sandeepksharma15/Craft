using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Craft.Security.CurrentUserService;

public class ApiUserProvider(IHttpContextAccessor httpContextAccessor) : ICurrentUserProvider
{
    public ClaimsPrincipal GetUser()
    {
        return httpContextAccessor?.HttpContext?.User;
    }
}
