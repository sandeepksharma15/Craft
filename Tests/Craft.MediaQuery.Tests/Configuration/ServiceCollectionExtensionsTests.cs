using Craft.MediaQuery.Configuration;
using Craft.MediaQuery.Models;
using Craft.MediaQuery.Services;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;

namespace Craft.MediaQuery.Tests.Configuration;

public class ServiceCollectionExtensionsTests
{
    [Fact]
    public void AddViewportResizeListener_WithConfiguration_ShouldConfigureOptions()
    {
        // Arrange
        var services = new ServiceCollection();
        var resizeOptions = new ResizeOptions { ReportRate = 500 };

        // Act
        services.AddViewportResizeListener(options => options.ReportRate = resizeOptions.ReportRate);

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var configuredOptions = serviceProvider.GetRequiredService<IOptions<ResizeOptions>>().Value;
        configuredOptions.Should().BeEquivalentTo(resizeOptions);
    }

    [Fact]
    public void AddViewportResizeListener_WithCustomConfigurationAction_ShouldApplyAction()
    {
        // Arrange
        var services = new ServiceCollection();
        var resizeOptions = new ResizeOptions { ReportRate = 500 };

        // Act
        services.AddViewportResizeListener(options =>
        {
            options.ReportRate = 1000;
            options.EnableLogging = true;
        });

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        var configuredOptions = serviceProvider.GetRequiredService<IOptions<ResizeOptions>>().Value;
        configuredOptions.Should().BeEquivalentTo(new ResizeOptions { ReportRate = 1000, EnableLogging = true });
    }

    [Fact]
    public void AddViewportResizeListener_ShouldConfigureServices()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddViewportResizeListener();

        // Assert
        services.Should().ContainSingle(descriptor => descriptor.ServiceType == typeof(IViewportResizeListener) && descriptor.Lifetime == ServiceLifetime.Scoped);
    }

    [Fact]
    public void AddViewportResizeListener_ShouldConfigureServicesWithCustomOptions()
    {
        // Arrange
        var services = new ServiceCollection();
        var resizeOptions = new ResizeOptions { ReportRate = 500, EnableLogging = true };

        // Act
        services.AddViewportResizeListener(opt => opt.ReportRate = resizeOptions.ReportRate);

        // Assert
        services.Should().ContainSingle(descriptor => descriptor.ServiceType == typeof(IViewportResizeListener) && descriptor.Lifetime == ServiceLifetime.Scoped);
    }
}
