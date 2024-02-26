using Craft.Logging.Logger;
using FluentAssertions;
using Serilog;
using Serilog.Events;
using Xunit;

namespace Craft.Logging.Tests;

public class CraftLoggerTests
{
    [Fact]
    public void EnsureInitialized_SetsMinimumLevels()
    {
        // Arrange
        CraftLogger.EnsureInitialized(); // Ensure logger is initialized

        // Act & Assert
        Log.Logger.IsEnabled(LogEventLevel.Debug).Should().BeTrue();
        Log.Logger.IsEnabled(LogEventLevel.Warning).Should().BeTrue();
    }
}
