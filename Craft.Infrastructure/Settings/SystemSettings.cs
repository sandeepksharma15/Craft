namespace Craft.Infrastructure.Settings;

public class SystemSettings
{
    public bool EnableExceptionMiddleware { get; set; } = true;
    public bool EnableHttpsLogging { get; set; } = false;
    public bool EnableSerilogRequestLogging { get; set; } = true;
}
