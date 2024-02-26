using Craft.Domain.HashIdentityKey;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Craft.Domain.Tests.Keys;

public class HashKeyTests
{
    [Fact]
    public void AddHashKeys_ResolvesHashKeysInstance()
    {
        // Arrange
        var services = new ServiceCollection();
        services.AddHashKeys();

        var serviceProvider = services.BuildServiceProvider();

        // Act
        var hashKeys = serviceProvider.GetService<IHashKeys>();

        // Assert
        hashKeys.Should().NotBeNull();
        hashKeys.Should().BeOfType<HashKeys>();
    }
}
