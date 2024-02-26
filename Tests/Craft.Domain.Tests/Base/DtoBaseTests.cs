using Craft.Domain.Base;
using FluentAssertions;
using Moq;

namespace Craft.Domain.Tests.Base;

public class DtoBaseTests
{
    [Fact]
    public void BaseDto_ConcurrencyStamp_ShouldSetAndGet()
    {
        // Arrange
        const string testConcurrencyStamp = "test-concurrency-stamp";
        var dto = new Mock<DtoBase>();

        //Act
        dto.SetupProperty(x => x.ConcurrencyStamp, testConcurrencyStamp);

        //Assert
        dto.Object.ConcurrencyStamp.Should().Be(testConcurrencyStamp);
    }

    [Fact]
    public void BaseDto_Id_ShouldSetAndGet()
    {
        // Arrange
        const int testId = 1;
        var dto = new Mock<DtoBase>();

        //Act
        dto.SetupProperty(x => x.Id, testId);

        //Assert
        dto.Object.Id.Should().Be(testId);
    }

    [Fact]
    public void BaseDto_IsDeleted_ShouldSetAndGet()
    {
        // Arrange
        const bool isDeleted = true;
        var dto = new Mock<DtoBase>();

        //Act
        dto.SetupProperty(x => x.IsDeleted, isDeleted);

        //Assert
        dto.Object.IsDeleted.Should().Be(isDeleted);
    }

    [Fact]
    public void ConcurrencyStamp_DefaultValue_ShouldBeNull()
    {
        // Arrange
        var dto = new TestDto();

        // Act
        var concurrencyStamp = dto.ConcurrencyStamp;

        // Assert
        concurrencyStamp.Should().BeNull();
    }

    [Fact]
    public void ConcurrencyStamp_SetValue_ShouldReturnSetValue()
    {
        // Arrange
        var dto = new TestDto();
        const string expectedConcurrencyStamp = "ABC123";

        // Act
        dto.ConcurrencyStamp = expectedConcurrencyStamp;
        var concurrencyStamp = dto.ConcurrencyStamp;

        // Assert
        concurrencyStamp.Should().Be(expectedConcurrencyStamp);
    }

    [Fact]
    public void Id_DefaultValue_ShouldBeNull()
    {
        // Arrange
        var dto = new TestDto();

        // Act
        var id = dto.Id;

        // Assert
        id.Should().Be(0);
    }

    [Fact]
    public void Id_SetValue_ShouldReturnSetValue()
    {
        // Arrange
        var dto = new TestDto();
        const int expectedId = 1;

        // Act
        dto.Id = expectedId;
        var id = dto.Id;

        // Assert
        id.Should().Be(expectedId);
    }

    [Fact]
    public void IsDeleted_DefaultValue_ShouldBeFalse()
    {
        // Arrange
        var dto = new TestDto();

        // Act
        var isDeleted = dto.IsDeleted;

        // Assert
        isDeleted.Should().BeFalse();
    }

    [Fact]
    public void IsDeleted_SetValue_ShouldReturnSetValue()
    {
        // Arrange
        var dto = new TestDto();
        const bool expectedIsDeleted = true;

        // Act
        dto.IsDeleted = expectedIsDeleted;
        var isDeleted = dto.IsDeleted;

        // Assert
        isDeleted.Should().Be(expectedIsDeleted);
    }

    // TestDto class for testing the abstract BaseDto
    public class TestDto : DtoBase;
}
