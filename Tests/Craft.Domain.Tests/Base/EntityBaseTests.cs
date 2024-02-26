using Craft.Domain.Base;
using Craft.Domain.Contracts;
using FluentAssertions;

namespace Craft.Domain.Tests.Base;

public class EntityBaseTests
{
    [Fact]
    public void ConcurrencyStamp_Should_Be_NewGuid()
    {
        // Arrange
        var entity = new MockEntity(1);

        // Act
        // Assert
        entity.ConcurrencyStamp.Should().NotBeNull();
        Guid.TryParse(entity.ConcurrencyStamp, out _).Should().BeTrue();
    }

    [Fact]
    public void EqualityOperator_Should_Return_False_For_Different_Id()
    {
        // Arrange
        var entity1 = new MockEntity(1);
        var entity2 = new MockEntity(2);

        // Act
        bool result = entity1 == entity2;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void EqualityOperator_Should_Return_True_For_Same_Id()
    {
        // Arrange
        var entity1 = new MockEntity(1);
        var entity2 = new MockEntity(1);

        // Act
        bool result = entity1 == entity2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentEntities()
    {
        // Arrange
        var entity1 = new MockEntity(1);
        var entity2 = new MockEntity(2);

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentTenants()
    {
        // Arrange
        var entity1 = new MockTenantEntity(1, 1001);
        var entity2 = new MockTenantEntity(1, 1002);

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentTypes()
    {
        // Arrange
        var entity = new MockEntity(1);
        var otherObject = new object();

        // Act
        var result = entity.Equals(otherObject);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenDifferentTypes_DerivedFromEntity()
    {
        // Arrange
        var entity = new MockEntity(1);
        var otherObject = new MockTenantEntity(1, 1);

        // Act
        var result = entity.Equals(otherObject);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnFalse_WhenNull()
    {
        // Arrange
        var entity = new MockEntity(1);

        // Act
        var result = entity.Equals(null);

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenEqualEntities()
    {
        // Arrange
        var entity1 = new MockEntity(1);
        var entity2 = new MockEntity(1);

        // Act
        var result = entity1.Equals(entity2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Equals_ShouldReturnTrue_WhenSameInstance()
    {
        // Arrange
        var entity = new MockEntity(1);

        // Act
        var result = entity.Equals(entity);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void GetHashCode_Should_Not_Return_Zero()
    {
        // Arrange
        var entity = new MockEntity(1);

        // Act
        // Assert
        entity.GetHashCode().Should().NotBe(0);
    }

    [Fact]
    public void GetHashCode_ShouldReturnDifferentValue_WhenDifferentEntities()
    {
        // Arrange
        var entity1 = new MockEntity(1);
        var entity2 = new MockEntity(2);

        // Act
        var hashCode1 = entity1.GetHashCode();
        var hashCode2 = entity2.GetHashCode();

        // Assert
        hashCode1.Should().NotBe(hashCode2);
    }

    [Fact]
    public void GetHashCode_ShouldReturnSameValue_WhenEqualEntities()
    {
        // Arrange
        var entity1 = new MockEntity(1);
        var entity2 = new MockEntity(1);

        // Act
        var hashCode1 = entity1.GetHashCode();
        var hashCode2 = entity2.GetHashCode();

        // Assert
        hashCode1.Should().Be(hashCode2);
    }

    [Fact]
    public void Id_Should_Have_Value()
    {
        // Arrange
        var entity = new MockEntity(1);
        const int expectedId = 1;

        // Act
        // entity.Id = expectedId;

        // Assert
        entity.Id.Should().Be(expectedId);
    }

    [Fact]
    public void InequalityOperator_Should_Return_False_For_Same_Id()
    {
        // Arrange
        var entity1 = new MockEntity(1);
        var entity2 = new MockEntity(1);

        // Act
        bool result = entity1 != entity2;

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void InequalityOperator_Should_Return_True_For_Different_Id()
    {
        // Arrange
        var entity1 = new MockEntity(1);
        var entity2 = new MockEntity(2);

        // Act
        bool result = entity1 != entity2;

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void IsDeleted_Should_Be_False_By_Default()
    {
        // Arrange
        var entity = new MockEntity(1);

        // Act

        // Assert
        entity.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void IsNew_Should_Return_False_For_Non_Default_Id()
    {
        // Arrange
        var entity = new MockEntity(1);

        // Act
        bool result = entity.IsNew();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void IsNew_Should_Return_True_For_Default_Id()
    {
        // Arrange
        var entity = new MockEntity();

        // Act
        bool result = entity.IsNew();

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void ToString_ShouldReturnExpectedString()
    {
        // Arrange
        var entity = new MockEntity(1);

        // Act
        var result = entity.ToString();

        // Assert
        result.Should().Be("[ENTITY: MockEntity] Key = 1");
    }

    private class MockEntity : EntityBase
    {
        public MockEntity()
        { }

        public MockEntity(int id) : base(id)
        {
        }
    }

    private class MockTenantEntity(long id, long tenantId) : EntityBase(id), IHasTenant
    {
        public KeyType TenantId { get; set; } = tenantId;
    }
}
