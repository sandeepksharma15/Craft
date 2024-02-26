using Craft.Domain.Base;
using FluentAssertions;
using Moq;

namespace Craft.Domain.Tests.Base;

public class VmBaseTests
{
    [Fact]
    public void BaseVm_ConcurrencyStamp_ShouldSetAndGet()
    {
        // Arrange
        const string testConcurrencyStamp = "test-concurrency-stamp";
        var Vm = new Mock<VmBase>();

        //Act
        Vm.SetupProperty(x => x.ConcurrencyStamp, testConcurrencyStamp);

        //Assert
        Vm.Object.ConcurrencyStamp.Should().Be(testConcurrencyStamp);
    }

    [Fact]
    public void BaseVm_Id_ShouldSetAndGet()
    {
        // Arrange
        const int testId = 1;
        var Vm = new Mock<VmBase>();

        //Act
        Vm.SetupProperty(x => x.Id, testId);

        //Assert
        Vm.Object.Id.Should().Be(testId);
    }

    [Fact]
    public void BaseVm_IsDeleted_ShouldSetAndGet()
    {
        // Arrange
        const bool isDeleted = true;
        var Vm = new Mock<VmBase>();

        //Act
        Vm.SetupProperty(x => x.IsDeleted, isDeleted);

        //Assert
        Vm.Object.IsDeleted.Should().Be(isDeleted);
    }

    [Fact]
    public void ConcurrencyStamp_DefaultValue_ShouldBeNull()
    {
        // Arrange
        var Vm = new TestVm();

        // Act
        var concurrencyStamp = Vm.ConcurrencyStamp;

        // Assert
        concurrencyStamp.Should().BeNull();
    }

    [Fact]
    public void ConcurrencyStamp_SetValue_ShouldReturnSetValue()
    {
        // Arrange
        var Vm = new TestVm();
        const string expectedConcurrencyStamp = "ABC123";

        // Act
        Vm.ConcurrencyStamp = expectedConcurrencyStamp;
        var concurrencyStamp = Vm.ConcurrencyStamp;

        // Assert
        concurrencyStamp.Should().Be(expectedConcurrencyStamp);
    }

    [Fact]
    public void Id_DefaultValue_ShouldBeNull()
    {
        // Arrange
        var Vm = new TestVm();

        // Act
        var id = Vm.Id;

        // Assert
        id.Should().Be(0);
    }

    [Fact]
    public void Id_SetValue_ShouldReturnSetValue()
    {
        // Arrange
        var Vm = new TestVm();
        const int expectedId = 1;

        // Act
        Vm.Id = expectedId;
        var id = Vm.Id;

        // Assert
        id.Should().Be(expectedId);
    }

    [Fact]
    public void IsDeleted_DefaultValue_ShouldBeFalse()
    {
        // Arrange
        var Vm = new TestVm();

        // Act
        var isDeleted = Vm.IsDeleted;

        // Assert
        isDeleted.Should().BeFalse();
    }

    [Fact]
    public void IsDeleted_SetValue_ShouldReturnSetValue()
    {
        // Arrange
        var Vm = new TestVm();
        const bool expectedIsDeleted = true;

        // Act
        Vm.IsDeleted = expectedIsDeleted;
        var isDeleted = Vm.IsDeleted;

        // Assert
        isDeleted.Should().Be(expectedIsDeleted);
    }

    public class TestVm : VmBase;
}
