using Craft.Infrastructure.Middleware;
using Craft.Infrastructure.Settings;
using Craft.Infrastructure.Tokens;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace AppFrame.Infrastructure;

public static class InfrastructureExtensions
{
    private static SystemSettings GetSystemSettings(IConfiguration config) =>
        config.GetSection(nameof(SystemSettings)).Get<SystemSettings>();

    public static IServiceCollection AddExceptionMiddleware(this IServiceCollection services, IConfiguration config)
    {
        if (GetSystemSettings(config).EnableExceptionMiddleware)
            services.AddScoped<ExceptionMiddleware>();

        return services;
    }

    public static IServiceCollection AddRequestLogging(this IServiceCollection services, IConfiguration config)
    {
        if (GetSystemSettings(config).EnableHttpsLogging)
        {
            services.AddSingleton<RequestLoggingMiddleware>();
            services.AddScoped<ResponseLoggingMiddleware>();
        }

        return services;
    }

    public static IServiceCollection AddTokenManager(this IServiceCollection services)
    {
        services.AddSingleton<ITokenManager, TokenManager>();

        return services;
    }

    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder app, IConfiguration config)
    {
        if (GetSystemSettings(config).EnableExceptionMiddleware)
            app.UseMiddleware<ExceptionMiddleware>();

        return app;
    }

    public static IApplicationBuilder UseRequestLogging(this IApplicationBuilder app, IConfiguration config)
    {
        if (GetSystemSettings(config).EnableHttpsLogging)
        {
            app.UseMiddleware<RequestLoggingMiddleware>();
            app.UseMiddleware<ResponseLoggingMiddleware>();
        }

        return app;
    }

    public static IApplicationBuilder UseSerilogHttpsLogging(this IApplicationBuilder app, IConfiguration config)
    {
        if (GetSystemSettings(config).EnableSerilogRequestLogging)
            app.UseSerilogRequestLogging(options =>
            {
                options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
                {
                    diagnosticContext.Set("RequestHost", httpContext.Request.Host);
                    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                    diagnosticContext.Set("RequestProtocol", httpContext.Request.Protocol);
                    diagnosticContext.Set("RequestPath", httpContext.Request.Path);
                    diagnosticContext.Set("RequestQueryString", httpContext.Request.QueryString);
                    diagnosticContext.Set("RequestContentType", httpContext.Request.ContentType);
                    diagnosticContext.Set("RequestContentLength", httpContext.Request.ContentLength);
                    diagnosticContext.Set("RequestHeaders", httpContext.Request.Headers);
                };
            });

        return app;
    }
}
