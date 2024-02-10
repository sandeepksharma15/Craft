using Craft.MediaQuery.Enums;

namespace Craft.MediaQuery.Models;

public sealed class ResizeOptions : IEquatable<ResizeOptions>
{
    public Dictionary<Breakpoint, int> Breakpoints { get; set; } = [];
    public bool EnableLogging { get; set; } = false;
    public bool NotifyOnBreakpointOnly { get; set; } = true;
    public int ReportRate { get; set; } = 250;
    public bool SuppressFirstEvent { get; set; } = false;

    public static bool operator !=(ResizeOptions left, ResizeOptions right) => !(left == right);

    public static bool operator ==(ResizeOptions left, ResizeOptions right)
    {
        if (ReferenceEquals(left, right)) return true;

        if (left is null) return false;
        if (right is null) return false;

        return left.Equals(right);
    }

    public override bool Equals(object obj) => obj is ResizeOptions options && Equals(options);

    public bool Equals(ResizeOptions other)
    {
        if (other is null)
            return false;

        if (ReferenceEquals(this, other))
            return true;

        if (ReportRate != other.ReportRate ||
            EnableLogging != other.EnableLogging ||
            SuppressFirstEvent != other.SuppressFirstEvent ||
            NotifyOnBreakpointOnly != other.NotifyOnBreakpointOnly)
            return false;

        if (Breakpoints is null)
            return other.Breakpoints is null;

        if (other.Breakpoints is null || Breakpoints.Count != other.Breakpoints.Count)
            return false;

        foreach (var breakpoint in Breakpoints.Keys)
            if (!other.Breakpoints.TryGetValue(breakpoint, out var otherWidth) || Breakpoints[breakpoint] != otherWidth)
                return false;

        return true;
    }

    public override int GetHashCode()
    {
        var hashCode = new HashCode();

        hashCode.Add(ReportRate);
        hashCode.Add(EnableLogging);
        hashCode.Add(SuppressFirstEvent);
        hashCode.Add(NotifyOnBreakpointOnly);
        hashCode.Add(ReportRate);

        if (Breakpoints is not null)
            foreach (var pair in Breakpoints)
            {
                hashCode.Add(pair.Key);
                hashCode.Add(pair.Value);
            }

        return hashCode.ToHashCode();
    }
}

public static class ResizeOptionsExtensions
{
    public static ResizeOptions Clone(this ResizeOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        return new ResizeOptions
        {
            Breakpoints = (options.Breakpoints ?? []).ToDictionary(entry => entry.Key, entry => entry.Value),
            EnableLogging = options.EnableLogging,
            NotifyOnBreakpointOnly = options.NotifyOnBreakpointOnly,
            ReportRate = options.ReportRate,
            SuppressFirstEvent = options.SuppressFirstEvent
        };
    }
}
