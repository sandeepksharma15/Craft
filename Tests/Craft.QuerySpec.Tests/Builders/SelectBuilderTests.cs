using Craft.QuerySpec.Builders;
using FluentAssertions;
using System.Linq.Expressions;

namespace Craft.QuerySpec.Tests.Builders;

public class SelectBuilderTests
{
    [Fact]
    public void AddColumn_Should_Add_Column_To_SelectList()
    {
        // Arrange
        Expression<Func<MyEntity, string>> assignor = s => s.Name;
        Expression<Func<MyResult, string>> assignee = d => d.Name;

        var builder = new SelectBuilder<MyEntity, MyResult>();

        // Act
        builder.Add(assignor, assignee);

        // Assert
        builder.Build().Should().NotBeNull();
        builder.Build().Parameters.Should().HaveCount(1);
        builder.Build().Parameters[0].Type.Should().Be(typeof(MyEntity));
        builder.Build().Body.Should().BeOfType<MemberInitExpression>();
    }

    [Fact]
    public void AddExpressionColumn_Should_Add_Column_To_SelectList()
    {
        // Arrange
        var builder = new SelectBuilder<MyEntity, MyResult>();

        // Act
        builder.Add(s => s.Name, d => d.Name);

        // Assert
        builder.Build().Should().NotBeNull();
        builder.Build().Parameters.Should().HaveCount(1);
        builder.Build().Parameters[0].Type.Should().Be(typeof(MyEntity));
        builder.Build().Body.Should().BeOfType<MemberInitExpression>();
    }

    [Fact]
    public void AddPropertyName_Should_Add_Column_To_SelectList()
    {
        // Arrange
        var builder = new SelectBuilder<MyEntity, MyResult>();

        // Act
        builder.Add("Name", "Name");

        // Assert
        builder.Build().Should().NotBeNull();
        builder.Build().Parameters.Should().HaveCount(1);
        builder.Build().Parameters[0].Type.Should().Be(typeof(MyEntity));
        builder.Build().Body.Should().BeOfType<MemberInitExpression>();
    }

    [Fact]
    public void AddExpressionColumn_Should_Add_Column_When_TResult_But_NoAssignTo_If_Same_Property()
    {
        // Arrange
        var builder = new SelectBuilder<MyEntity, MyResult>();

        // Act
        builder.Add(s => s.Name);

        // Assert
        builder.Build().Should().NotBeNull();
        builder.Build().Parameters.Should().HaveCount(1);
        builder.Build().Parameters[0].Type.Should().Be(typeof(MyEntity));
        builder.Build().Body.Should().BeOfType<MemberInitExpression>();
    }

    [Fact]
    public void BuildAnonymousSelect_ShouldConstructExpressionForObjectType()
    {
        // Arrange
        var data = new[]
        {
            new MyEntity { Id = 1, Name = "John", Age = 30 },
            new MyEntity { Id = 2, Name = "Alice", Age = 25 },
            new MyEntity { Id = 3, Name = "Bob", Age = 35 }
        }.AsQueryable();
        var selectBuilder = new SelectBuilder<MyEntity, object>()
            .Add(x => x.Name);

        // Act
        var selectExpression = selectBuilder.Build();
        var result = data.Select(selectExpression);

        // Assert
        selectExpression.Parameters.Should().HaveCount(1); // Expecting a single parameter
        result.Should().HaveCount(3);
    }

    [Fact]
    public void AddPropertyName_Should_Add_Column_When_TResult_But_NoAssignTo_If_Same_Property()
    {
        // Arrange
        var builder = new SelectBuilder<MyEntity, MyResult>();

        // Act
        builder.Add("Name");

        // Assert
        builder.Build().Should().NotBeNull();
        builder.Build().Parameters.Should().HaveCount(1);
        builder.Build().Parameters[0].Type.Should().Be(typeof(MyEntity));
        builder.Build().Body.Should().BeOfType<MemberInitExpression>();
    }

    [Fact]
    public void AddColumn_Should_Throw_Exception_When_TResult_But_NoAssignTo_If_Not_Same_Property()
    {
        // Arrange
        var builder = new SelectBuilder<MyEntity, MyResult>();
        Expression<Func<MyEntity, bool>> assignor = s => s.IsActive;

        // Act
        Action action = () => builder.Add(assignor);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddColumn_Should_Throw_Exception_When_TResult_But_AssignToIsNull()
    {
        // Arrange
        Expression<Func<MyEntity, string>> assignor = s => s.Name;

        var builder = new SelectBuilder<MyEntity, MyResult>();

        // Act
        Action action = () => builder.Add(assignor, null);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddColumn_Should_Throw_Exception_When_TResultSameAsT_But_AssignToIsPassed()
    {
        // Arrange
        Expression<Func<MyEntity, string>> assignor = s => s.Name;
        Expression<Func<MyResult, string>> assignee = d => d.Name;

        var builder = new SelectBuilder<MyEntity, MyEntity>();

        // Act
        Action action = () => builder.Add(assignor, assignee);

        // Assert
        action.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void Build_Should_Create_SelectPredicate_When_TResult_Equals_T()
    {
        // Arrange
        var builder = new SelectBuilder<MyEntity, MyEntity>();

        // Act
        var predicate = builder.Build();

        // Assert
        predicate.Should().NotBeNull();
        predicate.Parameters.Should().HaveCount(1);
        predicate.Parameters[0].Type.Should().Be(typeof(MyEntity));
    }

    [Fact]
    public void Build_Should_Create_SelectPredicateForResult_When_TResult_Not_T()
    {
        // Arrange
        Expression<Func<MyEntity, string>> assignor = s => s.Name;
        Expression<Func<MyResult, string>> assignee = d => d.Name;

        var builder = new SelectBuilder<MyEntity, MyResult>();

        // Act
        var predicate = builder.Add(assignor, assignee).Build();

        // Assert
        predicate.Should().NotBeNull();
        predicate.Parameters.Should().HaveCount(1);
        predicate.Parameters[0].Type.Should().Be(typeof(MyEntity));
    }

    [Fact]
    public void GetQuery_ShouldReturnQueryableWithSelectedProperties()
    {
        // Arrange
        Expression<Func<MyEntity, int>> idAssignor = s => s.Id;
        Expression<Func<MyResult, int>> idAssignee = d => d.Id;
        Expression<Func<MyEntity, string>> nameAssignor = s => s.Name;
        Expression<Func<MyResult, string>> nameAssignee = d => d.Name;

        var data = new[]
        {
            new MyEntity { Id = 1, Name = "John", Age = 30 },
            new MyEntity { Id = 2, Name = "Alice", Age = 25 },
            new MyEntity { Id = 3, Name = "Bob", Age = 35 }
        }.AsQueryable();

        var builder = new SelectBuilder<MyEntity, MyResult>()
            .Add(idAssignor, idAssignee)
            .Add(nameAssignor, nameAssignee);

        // Act
        var result = data.Select(builder.Build());

        // Assert
        result.Should().HaveCount(3);
    }

    [Fact]
    public void GetQuery_WithSameTAndTResult_ShouldReturnQueryableWithSelectedProperties()
    {
        // Arrange
        Expression<Func<MyEntity, int>> idAssignor = s => s.Id;
        Expression<Func<MyEntity, string>> nameAssignor = s => s.Name;
        var data = new[]
        {
            new MyEntity { Id = 1, Name = "John", Age = 30 },
            new MyEntity { Id = 2, Name = "Alice", Age = 25 },
            new MyEntity { Id = 3, Name = "Bob", Age = 35 }
        }.AsQueryable();

        var builder = new SelectBuilder<MyEntity>()
            .Add(idAssignor)
            .Add(nameAssignor);

        // Act
        var result = data.Select(builder.Build());

        // Assert
        result.Should().HaveCount(3);
    }

    [Fact]
    public void GetQuery_WithNonMatchingTAndTResult_ShouldReturnQueryableWithMappedProperties()
    {
        // Arrange
        Expression<Func<MyEntity, int>> idAssignor = s => s.Id;
        Expression<Func<MyEntity, string>> nameAssignor = s => s.Name;
        Expression<Func<MyEntity, int>> ageAssignor = s => s.Age;

        var data = new[]
        {
            new MyEntity { Id = 1, Name = "John", Age = 30 },
            new MyEntity { Id = 2, Name = "Alice", Age = 25 },
            new MyEntity { Id = 3, Name = "Bob", Age = 35 }
        }.AsQueryable();

        var builder = new SelectBuilder<MyEntity, MyResult>()
            .Add(idAssignor)
            .Add(nameAssignor)
            .Add(ageAssignor);

        // Act
        var result = data.Select(builder.Build());

        // Assert
        result.Should().HaveCount(3);
    }

    [Fact]
    public void Clear_ShouldRemoveAllSelectExpressions()
    {
        // Arrange
        Expression<Func<MyEntity, string>> assignor = s => s.Name;
        Expression<Func<MyResult, string>> assignee = d => d.Name;

        var builder = new SelectBuilder<MyEntity, MyResult>();

        // Act
        builder.Clear();

        // Assert
        builder.Count.Should().Be(0); // Expecting no select expressions after clearing
    }
}

public class MyEntity
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public bool IsActive { get; set; }
}

public class MyResult
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
