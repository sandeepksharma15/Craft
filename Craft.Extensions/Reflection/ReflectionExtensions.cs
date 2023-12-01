using System.Linq.Expressions;

namespace System.Reflection;

public static class ReflectionExtensions
{
    /// <summary>
    /// Gets the <see cref="PropertyInfo"/> of a specified property by name from the given <see cref="Type"/>.
    /// Throws an <see cref="ArgumentException"/> if the property is not found.
    /// </summary>
    /// <param name="objType">The type to search for the property.</param>
    /// <param name="name">The name of the property to retrieve.</param>
    /// <returns>The <see cref="PropertyInfo"/> of the specified property.</returns>
    public static PropertyInfo GetPropertyInfo(this Type objType, string name)
    {
        // Get all public instance and static properties of the specified type
        var properties = objType.GetProperties();

        // Find the property with the specified name
        var matchedProperty = Array.Find(properties, p => p.Name == name);

        // Throw an ArgumentException if the property is not found
        return matchedProperty ?? throw new ArgumentException($"Property '{name}' not found in type '{objType.FullName}'", nameof(name));
    }

    /// <summary>
    /// Gets the <see cref="PropertyInfo"/> from a property access expression represented by the given lambda expression.
    /// Throws an <see cref="ArgumentException"/> if the expression is not a valid property access expression.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object containing the property.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="expression">The lambda expression representing the property access.</param>
    /// <returns>The <see cref="PropertyInfo"/> of the accessed property.</returns>
    public static PropertyInfo GetPropertyInfo<TSource, TProperty>(this Expression<Func<TSource, TProperty>> expression)
    {
        if (expression.Body is MemberExpression memberExpression && memberExpression.Member is PropertyInfo propertyInfo)
            return propertyInfo;

        throw new ArgumentException("Invalid expression. Expected a property access expression.");
    }

    // Summary Comment:
    // This extension method, designed for lambda expressions representing property access,
    // extracts the PropertyInfo of the accessed property. It supports both direct property access
    // and nested property access within UnaryExpression. If the expression is not valid for property access,
    // it throws an ArgumentException to indicate an invalid expression.
    public static PropertyInfo GetPropertyInfo<T>(this Expression<Func<T, object>> expression)
    {
        if (expression.Body is MemberExpression memberExpression)
        {
            if (memberExpression.Member is PropertyInfo propertyInfo)
                return propertyInfo;
        }
        else if (expression.Body is UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MemberExpression nestedMemberExpression && nestedMemberExpression.Member is PropertyInfo nestedPropertyInfo)
                return nestedPropertyInfo;
        }

        throw new ArgumentException("Invalid expression. Expected a property access expression.");
    }
}
