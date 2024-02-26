using Craft.Domain.Contracts;
using Craft.Domain.Helpers;
using FluentAssertions;

namespace Domain.Tests.Base;

public class EntityHelperTests
{
    [Fact]
    public void EntityEquals_WithDifferentEntities_ShouldReturnFalse()
    {
        // Arrange
        IEntity entity1 = new TestEntity { Id = 1 };
        IEntity entity2 = new TestEntity { Id = 2 };

        // Act
        bool result = EntityHelper.EntityEquals(entity1, entity2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EntityEquals_WithEqualEntities_ShouldReturnTrue()
    {
        // Arrange
        IEntity entity1 = new TestEntity { Id = 1 };
        IEntity entity2 = new TestEntity { Id = 1 };

        // Act
        bool result = EntityHelper.EntityEquals(entity1, entity2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void EntityEquals_WithNullEntity1_ShouldReturnFalse()
    {
        // Arrange
        IEntity entity1 = null;
        IEntity entity2 = new TestEntity { Id = 1 };

        // Act
        bool result = EntityHelper.EntityEquals(entity1, entity2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EntityEquals_WithNullEntity2_ShouldReturnFalse()
    {
        // Arrange
        IEntity entity1 = new TestEntity { Id = 1 };
        IEntity entity2 = null;

        // Act
        bool result = EntityHelper.EntityEquals(entity1, entity2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EntityEquals_WithSameReference_ShouldReturnTrue()
    {
        // Arrange
        IEntity entity1 = new TestEntity { Id = 1 };
        IEntity entity2 = entity1;

        // Act
        bool result = EntityHelper.EntityEquals(entity1, entity2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasDefaultId_ShouldReturnFalse_WhenEntityIdIsNotDefault()
    {
        // Arrange
        var entity = new FakeEntity<int> { Id = 1 };

        // Act
        var result = EntityHelper.HasDefaultId(entity);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HasDefaultId_ShouldReturnFalse_WhenEntityIdIsPositiveInt()
    {
        // Arrange
        var entity = new FakeEntity<int> { Id = 10 };

        // Act
        var result = EntityHelper.HasDefaultId(entity);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HasDefaultId_ShouldReturnFalse_WhenEntityIdIsPositiveLong()
    {
        // Arrange
        var entity = new FakeEntity<long> { Id = 1 };

        // Act
        var result = EntityHelper.HasDefaultId(entity);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void HasDefaultId_ShouldReturnTrue_WhenEntityIdIsDefault()
    {
        // Arrange
        var entity = new FakeEntity<int> { Id = default };

        // Act
        var result = EntityHelper.HasDefaultId(entity);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasDefaultId_ShouldReturnTrue_WhenEntityIdIsNegativeInt()
    {
        // Arrange
        var entity = new FakeEntity<int> { Id = -1 };

        // Act
        var result = EntityHelper.HasDefaultId(entity);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void HasDefaultId_ShouldReturnTrue_WhenEntityIdIsNegativeLong()
    {
        // Arrange
        var entity = new FakeEntity<long> { Id = -1 };

        // Act
        var result = EntityHelper.HasDefaultId(entity);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsEntity_WithEntity_ShouldReturnTrue()
    {
        // Arrange
        Type type = typeof(TestEntity);

        // Act
        bool result = EntityHelper.IsEntity(type);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsEntity_WithNonEntity_ShouldReturnFalse()
    {
        // Arrange
        Type type = typeof(NonEntityClass);

        // Act
        bool result = EntityHelper.IsEntity(type);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsEntity_WithNullType_ShouldThrowArgumentNullException()
    {
        // Arrange
        Type type = null;

        // Act
        Action action = () => EntityHelper.IsEntity(type);

        // Assert
        action.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void IsMultiTenant_WithMultiTenantEntity_ShouldReturnTrue()
    {
        // Arrange
        const bool expectedResult = true;

        // Act
        bool result = EntityHelper.IsMultiTenant<MultiTenantEntity>();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void IsMultiTenant_WithNonMultiTenantEntity_ShouldReturnFalse()
    {
        // Arrange
        const bool expectedResult = false;

        // Act
        bool result = EntityHelper.IsMultiTenant<NonMultiTenantEntity>();

        // Assert
        result.Should().Be(expectedResult);
    }

    [Fact]
    public void IsMultiTenant_WithNullType_ShouldReturnFalse()
    {
        // Arrange
        Type type = null;

        // Act
        bool result = EntityHelper.IsMultiTenant(type);

        // Assert
        result.Should().Be(false);
    }
}

public class MultiTenantEntity : IEntity, IHasTenant
{
    public KeyType Id { get; set; }
    public KeyType TenantId { get; set; }
}

public class NonEntityClass
{
    public KeyType Id { get; set; }
}

public class NonMultiTenantEntity : IEntity
{
    public KeyType Id { get; set; }
}

public class TestEntity : IEntity
{
    public KeyType Id { get; set; }
}

public class FakeEntity<TKey> : IEntity<TKey>
{
    public TKey Id { get; set; }
}
