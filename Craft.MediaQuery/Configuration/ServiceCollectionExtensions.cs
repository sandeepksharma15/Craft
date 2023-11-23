using Craft.MediaQuery.Models;
using Craft.MediaQuery.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Craft.MediaQuery.Configuration;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddViewportResizeListener(this IServiceCollection services,
        Action<ResizeOptions> resizeOptions = null)
    {
        services.Configure(resizeOptions ?? (_ => { }));

        services.AddScoped<IViewportResizeListener, ViewportResizeListener>();

        return services;
    }

    public static IServiceCollection AddContainerObserver(this IServiceCollection services,
        Action<ResizeOptions> resizeOptions = null)
    {
        services.Configure(resizeOptions ?? (_ => { }));

        services.AddScoped<IContainerResizeListener, ContainerResizeListener>();

        return services;
    }
}
