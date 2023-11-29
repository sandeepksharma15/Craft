using Craft.JsHelpers.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Craft.JsHelpers.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommonJsFunctionsProvider(this IServiceCollection services)
    {
        services.AddScoped<CommonJsFunctionsProvider>();

        return services;
    }
}
