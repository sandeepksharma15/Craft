using Craft.Domain.Contracts;
using FluentAssertions;

namespace Craft.Domain.Tests.Contracts;

public class SoftDeleteTests
{
    [Fact]
    public void ISoftDelete_ColumnName_IsConstant()
    {
        // Assert
        ISoftDelete.ColumnName.Should().Be("IsDeleted");
    }

    [Fact]
    public void IsDeleted_ReturnsFalseInitially()
    {
        // Arrange
        ISoftDelete instance = new ConcreteSoftDelete();

        // Act & Assert
        instance.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Delete_SetsIsDeletedToTrue()
    {
        // Arrange
        ISoftDelete instance = new ConcreteSoftDelete();

        // Act
        instance.Delete();

        // Assert
        instance.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void Restore_SetsIsDeletedToFalse()
    {
        // Arrange
        ISoftDelete instance = new ConcreteSoftDelete();
        instance.Delete(); // Set IsDeleted to true first

        // Act
        instance.Restore();

        // Assert
        instance.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void Restore_HasNoEffectOnAlreadyRestoredObject()
    {
        // Arrange
        ISoftDelete instance = new ConcreteSoftDelete();

        // Act & Assert
        instance.Restore(); // Should have no effect

        // Assert
        instance.IsDeleted.Should().BeFalse(); // Remains false
    }

    private class ConcreteSoftDelete : ISoftDelete
    {
        public bool IsDeleted { get; set; }
    }
}
