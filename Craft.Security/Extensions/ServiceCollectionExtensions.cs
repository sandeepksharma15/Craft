using Craft.Security.CurrentUserService;
using Microsoft.Extensions.DependencyInjection;

namespace Craft.Security.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCurrentApiUser(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserProvider, ApiUserProvider>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }

    public static IServiceCollection AddCurrentUiUser(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUserProvider, UiUserProvider>();
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}
