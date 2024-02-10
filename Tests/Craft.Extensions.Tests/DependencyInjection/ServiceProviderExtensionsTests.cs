using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;

namespace Craft.Extensions.Tests.DependencyInjection;

internal interface ITestService;

public class ServiceProviderExtensionsTests
{
    [Fact]
    public void AddService_WithValidLifetimeAndTypes_AddsServiceToCollection()
    {
        // Arrange
        var services = new ServiceCollection();
        var serviceType = typeof(ITestService);
        var implementationType = typeof(TestService);

        // Act
        services.AddService(serviceType, implementationType, ServiceLifetime.Transient);

        // Assert
        services.IsAdded(serviceType).Should().BeTrue();
        services.IsImplementationAdded(implementationType).Should().BeTrue();
    }

    [Fact]
    public void AddServices_WithInvalidInterfaceType_DoesNotRegisterTypes()
    {
        // Arrange
        var services = new ServiceCollection();
        var interfaceType = typeof(IDisposable); // Not implemented by any test classes

        // Act
        services.AddServices(interfaceType, ServiceLifetime.Transient);

        // Assert
        services.IsImplementationAdded(typeof(TestService1)).Should().BeFalse();
        services.IsImplementationAdded(typeof(TestService2)).Should().BeFalse();
    }

    [Fact]
    public void AddServices_WithValidInterfaceType_RegistersImplementations()
    {
        // Arrange
        var services = new ServiceCollection();
        var interfaceType = typeof(ITestService);

        // Act
        services.AddServices(interfaceType, ServiceLifetime.Transient);

        // Assert
        services.IsImplementationAdded(typeof(TestService1)).Should().BeTrue();
        services.IsImplementationAdded(typeof(TestService2)).Should().BeTrue();
    }

    [Fact]
    public void GetSingletonInstance_WithNonSingletonService_ThrowsInvalidOperationException()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<TestService>();

        // Act and Assert
        Action act = () => services.GetSingletonInstance<TestService>();
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetSingletonInstance_WithSingletonService_ReturnsInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        var singletonService = new TestService();
        services.AddSingleton(singletonService);

        // Act
        var retrievedInstance = services.GetSingletonInstance<TestService>();

        // Assert
        retrievedInstance.Should().BeSameAs(singletonService);
    }

    [Fact]
    public void GetSingletonInstanceOrNull_WithNonSingletonService_ReturnsNull()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<TestService>();

        // Act
        var retrievedInstance = services.GetSingletonInstanceOrNull<TestService>();

        // Assert
        retrievedInstance.Should().BeNull();
    }

    [Fact]
    public void GetSingletonInstanceOrNull_WithSingletonService_ReturnsInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        var singletonService = new TestService();
        services.AddSingleton(singletonService);

        // Act
        var retrievedInstance = services.GetSingletonInstanceOrNull<TestService>();

        // Assert
        retrievedInstance.Should().BeSameAs(singletonService);
    }

    [Fact]
    public void IsAdded_WithNonRegisteredService_ReturnsFalse()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        var isAdded = services.IsAdded<TestService>();

        // Assert
        isAdded.Should().BeFalse();
    }

    [Fact]
    public void IsAdded_WithRegisteredService_ReturnsTrue()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<TestService>();

        // Act
        var isAdded = services.IsAdded<TestService>();

        // Assert
        isAdded.Should().BeTrue();
    }

    [Fact]
    public void IsImplementationAdded_WithRegisteredImplementation_ReturnsTrue()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<TestService>();

        // Act
        var isImplementationAdded = services.IsImplementationAdded<TestService>();

        // Assert
        isImplementationAdded.Should().BeTrue();
    }

    [Fact]
    public void IsImplementationAdded_WithRegisteredInterface_ReturnsFalse()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddSingleton<ITestService>(new TestService1());

        // Act
        var isImplementationAdded = services.IsImplementationAdded<TestService>();

        // Assert
        isImplementationAdded.Should().BeFalse();
    }

    [Fact]
    public void ResolveWith_WithRegisteredServiceAndNoParameters_ResolvesInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<TestService3>();
        var provider = services.BuildServiceProvider();

        // Act
        var resolvedInstance = provider.ResolveWith<TestService>();

        // Assert
        resolvedInstance.Should().NotBeNull();
    }

    [Fact]
    public void ResolveWith_WithRegisteredServiceAndParameters_ResolvesInstanceWithInjectedParameters()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddTransient<TestService3>();
        services.AddTransient<ITestService, TestService>();
        var provider = services.BuildServiceProvider();
        var dependency = provider.GetService<ITestService>();

        // Act
        var resolvedInstance = provider.ResolveWith<TestService3>(dependency);

        // Assert
        resolvedInstance.Should().NotBeNull();
        resolvedInstance.TestService.Should().BeSameAs(dependency);
    }
}

internal class TestService : ITestService;

internal class TestService1 : ITestService;

internal class TestService2 : ITestService;

internal class TestService3(ITestService testService = null)
{
    public ITestService TestService { get; set; } = testService;
}
