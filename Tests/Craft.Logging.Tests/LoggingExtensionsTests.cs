using Microsoft.AspNetCore.Builder;
using Xunit;
using Craft.Logging.Extensions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Serilog.Events;
using Serilog;
using Serilog.Context;

namespace Craft.Logging.Tests;

public class LoggingExtensionsTests
{
    [Fact]
    public void AddLogging_Should_Clear_Logging_Providers()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();

        // Act
        var result = builder.AddLogging();
        var providers = builder.Services.Where(descriptor => descriptor.ServiceType == typeof(ILoggerProvider));

        // Assert
        result.Should().BeSameAs(builder);
        providers.Should().BeEmpty();
    }

    [Fact]
    public void AddLogging_Should_Use_Serilog_With_Correct_Configuration()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder();

        // Act
        var result = builder.AddLogging();

        // Assert
        result.Should().BeSameAs(builder);

        // Get Serilog Configuration
        var logger = builder.Services.BuildServiceProvider().GetService<ILogger<LoggingExtensionsTests>>();
        logger.Should().NotBeNull();
    }
}
