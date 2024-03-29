﻿using System.Data;
using System.Linq.Expressions;
using System.Text.Json;
using Craft.QuerySpec.Builders;
using Craft.QuerySpec.Enums;
using Craft.QuerySpec.Helpers;
using Craft.TestHelper.Models;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Builders;

public class EntityFilterBuilderTests
{
    private readonly JsonSerializerOptions serializeOptions;
    private readonly IQueryable<Company> queryable;

    public EntityFilterBuilderTests()
    {
        serializeOptions = new JsonSerializerOptions();
        serializeOptions.Converters.Add(new EntityFilterBuilderJsonConverter<Company>());

        queryable = new List<Company>
        {
            new() { Id = 1, Name = "Company 1" },
            new() { Id = 2, Name = "Company 2" }
        }.AsQueryable();
    }

    [Fact]
    public void Clear_EmptiesWhereExpressions()
    {
        // Arrange
        var whereBuilder = new EntityFilterBuilder<Company>();
        whereBuilder.Add(u => u.Name == "Company 1");

        // Act
        var result = whereBuilder.Clear();

        // Assert
        result.Should().Be(whereBuilder);
        whereBuilder.EntityFilterList.Should().BeEmpty();
    }

    [Fact]
    public void Add_Expression_AddsToWhereExpressions()
    {
        // Arrange
        var whereBuilder = new EntityFilterBuilder<Company>();
        Expression<Func<Company, bool>> expression = u => u.Name == "Company 1";

        // Act
        var result = whereBuilder.Add(expression);
        var expr = whereBuilder.EntityFilterList[0];
        var filtered = queryable.Where(expr.Filter).ToList();

        // Assert
        result.Should().Be(whereBuilder);

        filtered.Should().NotBeNull();
        filtered.Should().HaveCount(1);
        filtered[0].Name.Should().Be("Company 1");
    }

    [Fact]
    public void Add_ExpressionWithProperties_ReturnsWhereExpressions()
    {
        // Arrange
        var whereBuilder = new EntityFilterBuilder<Company>();
        Expression<Func<Company, object>> propExpr = u => u.Name;
        object compareWith = "Company 1";

        // Act
        var result = whereBuilder.Add(propExpr, compareWith, ComparisonType.EqualTo);
        var expr = whereBuilder.EntityFilterList[0];
        var filtered = queryable.Where(expr.Filter).ToList();

        // Assert
        result.Should().Be(whereBuilder);
        whereBuilder.EntityFilterList.Should().NotBeEmpty();

        filtered.Should().NotBeNull();
        filtered.Should().HaveCount(1);
        filtered[0].Name.Should().Be("Company 1");
    }

    [Fact]
    public void Add_StringPropName_ReturnsWhereExpressions()
    {
        // Arrange
        var whereBuilder = new EntityFilterBuilder<Company>();
        const string propName = "Name";
        object compareWith = "Company 1";

        // Act
        var result = whereBuilder.Add(propName, compareWith, ComparisonType.NotEqualTo);
        var expr = whereBuilder.EntityFilterList[0];
        var filtered = queryable.Where(expr.Filter).ToList();

        // Assert
        result.Should().Be(whereBuilder);
        whereBuilder.EntityFilterList.Should().NotBeEmpty();

        filtered.Should().NotBeNull();
        filtered.Should().HaveCount(1);
        filtered[0].Name.Should().Be("Company 2");
    }

    [Fact]
    public void Remove_WhenExpressionFound_ShouldRemoveFromWhereExpressions()
    {
        // Arrange
        var whereBuilder = new EntityFilterBuilder<Company>();
        Expression<Func<Company, bool>> expression = t => t.Id == 1;
        whereBuilder.Add(expression);

        // Act
        whereBuilder.Remove(expression);

        // Assert
        whereBuilder.EntityFilterList.Should().BeEmpty();
    }

    [Fact]
    public void Remove_WhenExpressionNotFound_ShouldNotChangeWhereExpressions()
    {
        // Arrange
        var whereBuilder = new EntityFilterBuilder<Company>();
        Expression<Func<Company, bool>> expression = t => t.Id == 1;
        whereBuilder.Add(t => t.Name == "Test");

        // Act
        whereBuilder.Remove(expression);

        // Assert
        whereBuilder.EntityFilterList.Should().NotBeEmpty();
    }

    [Fact]
    public void Remove_WhenPropertyExpressionFound_ShouldRemoveFromWhereExpressions()
    {
        // Arrange
        var whereBuilder = new EntityFilterBuilder<Company>();
        Expression<Func<Company, object>> propExpr = u => u.Name;
        object compareWith = "Company 1";
        whereBuilder.Add(propExpr, compareWith, ComparisonType.EqualTo);

        // Act
        whereBuilder.Remove(propExpr, compareWith, ComparisonType.EqualTo);

        // Assert
        whereBuilder.EntityFilterList.Should().BeEmpty();
    }

    [Fact]
    public void Remove_WhenPropertyNameExpressionFound_ShouldRemoveFromWhereExpressions()
    {
        // Arrange
        // Arrange
        var whereBuilder = new EntityFilterBuilder<Company>();
        const string propName = "Name";
        object compareWith = "Company 1";

        whereBuilder.Add(propName, compareWith, ComparisonType.NotEqualTo);

        // Act
        whereBuilder.Remove(propName, compareWith, ComparisonType.NotEqualTo);

        // Assert
        whereBuilder.EntityFilterList.Should().BeEmpty();
    }

    [Fact]
    public void CanConvert_ReturnsTrueForCorrectType()
    {
        var converter = new EntityFilterBuilderJsonConverter<TestClass>();
        bool canConvert = converter.CanConvert(typeof(EntityFilterBuilder<TestClass>));

        canConvert.Should().BeTrue();
    }

    [Fact]
    public void CanConvert_ReturnsFalseForIncorrectType()
    {
        var converter = new EntityFilterBuilderJsonConverter<TestClass>();
        bool canConvert = converter.CanConvert(typeof(string));

        canConvert.Should().BeFalse();
    }

    [Fact]
    public void Read_DeserializesValidJsonToEntityFilterBuilder()
    {
        // Arrange
        const string validJson = "[{\"Filter\": \"Name == 'John'\"}]";

        // Act
        var filterBuilder = JsonSerializer.Deserialize<EntityFilterBuilder<Company>>(validJson, serializeOptions);

        // Assert
        filterBuilder.EntityFilterList.Count.Should().Be(1);
        filterBuilder.EntityFilterList[0].Filter.Body.ToString().Should().Contain("x.Name");
    }

    [Fact]
    public void Read_ThrowsJsonExceptionForInvalidFormat()
    {
        // Arrange
        const string invalidJson = "{}"; // Not an array

        // Act and Assert
        Action act = () => JsonSerializer.Deserialize<EntityFilterBuilder<Company>>(invalidJson, serializeOptions);

        act.Should().Throw<JsonException>();
    }

    [Fact]
    public void Write_SerializesEntityFilterBuilderToJsonCorrectly()
    {
        // Arrange
        var filterBuilder = new EntityFilterBuilder<Company>();
        Expression<Func<Company, bool>> filterCriteria = x => x.Name == "John";
        filterBuilder.Add(filterCriteria);

        // Act
        var json = JsonSerializer.Serialize(filterBuilder, serializeOptions);

        // Assert
        json.Should().Be("[{\"Filter\":\"(Name == \\u0022John\\u0022)\"}]");
    }
}
