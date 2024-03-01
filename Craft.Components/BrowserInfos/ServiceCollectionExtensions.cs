using Microsoft.Extensions.DependencyInjection;

namespace Craft.Components.BrowserInfos;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBrowserInfo(this IServiceCollection services)
    {
        return services.AddScoped<IBrowserInfoService, BrowserInfoService>();
    }
}
