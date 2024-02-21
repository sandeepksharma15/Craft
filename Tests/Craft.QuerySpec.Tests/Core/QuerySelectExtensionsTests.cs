﻿using System.Linq.Expressions;
using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Core;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Core;

public class QuerySelectExtensionsTests
{
    [Fact]
    public void Select_Should_AddColumnToQuerySelectBuilder()
    {
        // Arrange
        var query = new Query<Company, Company>();
        Expression<Func<Company, object>> column = x => x.Name;

        // Act
        var result = query.Select(column);

        // Assert
        result.Should().BeSameAs(query);
        result.QuerySelectBuilder.SelectDescriptorList.Should().HaveCount(1);
        result.QuerySelectBuilder.SelectDescriptorList[0].Assignor.Body.ToString().Should().Contain("Name");
        result.QuerySelectBuilder.SelectDescriptorList[0].Assignee.Body.ToString().Should().Contain("Name");
    }

    [Fact]
    public void Select_Should_ReturnQueryUnchanged_WhenQueryIsNull()
    {
        // Arrange
        IQuery<Company, Company> query = null;
        Expression<Func<Company, object>> column = x => x.Name;

        // Act
        var result = query.Select(column);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Select_Should_ReturnQueryUnchanged_WhenColumnIsNull()
    {
        // Arrange
        var query = new Query<Company, Company>();
        Expression<Func<Company, object>> column = null;

        // Act
        var result = query.Select(column);

        // Assert
        result.Should().BeSameAs(query);
    }

    [Fact]
    public void Select_Should_AddAssignorAndAssigneeToQuerySelectBuilder()
    {
        // Arrange
        var query = new Query<Company, MyResult>();
        Expression<Func<Company, object>> assignor = x => x.Name;
        Expression<Func<MyResult, object>> assignee = x => x.Name;

        // Act
        var result = query.Select(assignor, assignee);

        // Assert
        result.Should().BeSameAs(query);
        result.QuerySelectBuilder.SelectDescriptorList.Should().HaveCount(1);
        result.QuerySelectBuilder.SelectDescriptorList[0].Assignor.Body.ToString().Should().Contain("Name");
        result.QuerySelectBuilder.SelectDescriptorList[0].Assignee.Body.ToString().Should().Contain("Name");
    }

    [Fact]
    public void Select_Should_AddAssignorPropNameToQuerySelectBuilder()
    {
        // Arrange
        var query = new Query<Company, MyResult>();

        // Act
        var result = query.Select("Name");

        // Assert
        result.Should().BeSameAs(query);
        result.QuerySelectBuilder.SelectDescriptorList.Should().HaveCount(1);
        result.QuerySelectBuilder.SelectDescriptorList[0].Assignor.Body.ToString().Should().Contain("Name");
        result.QuerySelectBuilder.SelectDescriptorList[0].Assignee.Body.ToString().Should().Contain("Name");
    }

    [Fact]
    public void Select_Should_AddAssignorPropNameAndAssigneePropNameToQuerySelectBuilder()
    {
        // Arrange
        var query = new Query<Company, MyResult>();

        // Act
        var result = query.Select("Name", "Name");

        // Assert
        result.Should().BeSameAs(query);
        result.QuerySelectBuilder.SelectDescriptorList.Should().HaveCount(1);
        result.QuerySelectBuilder.SelectDescriptorList[0].Assignor.Body.ToString().Should().Contain("Name");
        result.QuerySelectBuilder.SelectDescriptorList[0].Assignee.Body.ToString().Should().Contain("Name");
    }
}

public class MyResult
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
}
