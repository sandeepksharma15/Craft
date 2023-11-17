using Craft.MediaQuery.Enums;

namespace Craft.MediaQuery.Models;

public static class GlobalOptions
{
    public static Dictionary<Breakpoint, int> DefaultBreakpoints { get; set; } = new()
    {
        [Breakpoint.FullHD] = 1920,
        [Breakpoint.Widescreen] = 1600,
        [Breakpoint.Desktop] = 1200,
        [Breakpoint.Tablet] = 900,
        [Breakpoint.Mobile] = 600,
        [Breakpoint.ExtraSmall] = 0,
    };

    public static Dictionary<Breakpoint, int> GetDefaultBreakpoints()
        => DefaultBreakpoints.ToDictionary(entry => entry.Key, entry => entry.Value);

    public static Dictionary<Breakpoint, int> GetBreakpoints(ResizeOptions options)
       => (options?.Breakpoints?.Count ?? 0) > 0
           ? options.Breakpoints.ToDictionary(entry => entry.Key, entry => entry.Value)
           : DefaultBreakpoints.ToDictionary(entry => entry.Key, entry => entry.Value);

    public static ResizeOptions GetDefaultResizeOptions()
    {
        return new()
        {
            Breakpoints = GetBreakpoints(null),
            EnableLogging = true,
            NotifyOnBreakpointOnly = true,
            ReportRate = 300,
            SuppressFirstEvent = true
        };
    }
}
