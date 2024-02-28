using Microsoft.AspNetCore.Builder;

namespace Craft.Logging.Serilog;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseAbpSerilogEnrichers(this IApplicationBuilder app)
        => app.UseMiddleware<SerilogMiddleware>();
}
