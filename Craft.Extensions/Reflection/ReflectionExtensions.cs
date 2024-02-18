using System.ComponentModel;
using System.Linq.Expressions;

namespace System.Reflection;

public static class ReflectionExtensions
{
    /// <summary>
    /// Gets a PropertyDescriptor for a specified member by name within the given Type.
    /// Supports nested properties using dot notation (e.g., "NestedClass.Property").
    /// </summary>
    /// <param name="type">The Type containing the member.</param>
    /// <param name="memberName">The name of the member to retrieve.</param>
    /// <returns>A PropertyDescriptor for the specified member or null if not found.</returns>
    public static PropertyDescriptor GetMemberByName(this Type type, string memberName)
    {
        // Get all properties for the given type
        var members = TypeDescriptor.GetProperties(type);

        // If the memberName does not contain a dot, find the property directly
        if (!memberName.Contains('.'))
            return members.Find(memberName, true);

        // If the memberName contains a dot, consider it as a nested property
        var memberNameParts = memberName.Split('.');
        var topLevelMember = members.Find(memberNameParts[0], true);

        // If the top-level member is found, get child properties and find the nested property
        return topLevelMember?.GetChildProperties()?.Find(memberNameParts[1], true);
    }

    /// <summary>
    /// Retrieves the name of a property from a lambda expression.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="property">The lambda expression representing the property.</param>
    /// <returns>The name of the property.</returns>
    public static string GetMemberName<TSource, TProperty>(this Expression<Func<TSource, TProperty>> property)
        => property.GetPropertyInfo().Name;

    /// <summary>
    /// Gets the underlying type of the member represented by the lambda expression.
    /// </summary>
    /// <typeparam name="TSource">The type of the source object.</typeparam>
    /// <typeparam name="TProperty">The type of the property.</typeparam>
    /// <param name="property">The lambda expression representing the property.</param>
    /// <returns>The underlying type of the member, handling nullable types appropriately.</returns>
    public static Type GetMemberType<TSource, TProperty>(this Expression<Func<TSource, TProperty>> property)
    {
        // Get the PropertyInfo object for the expression and then retrieve the underlying type
        var type = property.GetPropertyInfo().GetMemberUnderlyingType();

        // Handle nullable types by returning the underlying type if the type is Nullable<>
        if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            return Nullable.GetUnderlyingType(type);

        return type;
    }

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
        if (expression.Body is MemberExpression memberExpression)
        {
            if (memberExpression.Member is PropertyInfo propertyInfo)
                return propertyInfo;
        }
        else if (expression.Body is UnaryExpression unaryExpression &&
                    unaryExpression.Operand is MemberExpression nestedMemberExpression &&
                    nestedMemberExpression.Member is PropertyInfo nestedPropertyInfo)
        {
            return nestedPropertyInfo;
        }

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
        else if (expression.Body is UnaryExpression unaryExpression &&
                    unaryExpression.Operand is MemberExpression nestedMemberExpression &&
                    nestedMemberExpression.Member is PropertyInfo nestedPropertyInfo)
        {
            return nestedPropertyInfo;
        }

        throw new ArgumentException("Invalid expression. Expected a property access expression.");
    }

    /// <summary>
    /// Retrieves the PropertyInfo object from a lambda expression representing a property access.
    /// </summary>
    /// <param name="expression">The lambda expression.</param>
    /// <returns>The PropertyInfo object.</returns>
    public static PropertyInfo GetPropertyInfo(this LambdaExpression expression)
    {
        // Check if the expression body is a MemberExpression
        if (expression.Body is MemberExpression memberExpression)
        {
            // If it is, check if the member is a PropertyInfo
            if (memberExpression.Member is PropertyInfo propertyInfo)
                return propertyInfo;
        }
        // Check if the expression body is a UnaryExpression with a nested MemberExpression
        else if (expression.Body is UnaryExpression unaryExpression &&
                    unaryExpression.Operand is MemberExpression nestedMemberExpression &&
                    nestedMemberExpression.Member is PropertyInfo nestedPropertyInfo)
        {
            return nestedPropertyInfo;
        }

        // If the expression does not match expected patterns, throw an ArgumentException
        throw new ArgumentException("Invalid expression. Expected a property access expression.");
    }

    public static T GetClone<T>(this T input)
    {
        if (input == null)
            throw new ArgumentNullException(nameof(input), "Input object cannot be null.");

        var type = typeof(T);
        var visited = new Dictionary<object, object>();
        return (T)CloneObject(input, type, visited);
    }

    private static object CloneObject(object input, Type type, Dictionary<object, object> visited)
    {
        if (visited.TryGetValue(input, out object keyValue))
            return keyValue;

        var clonedObj = Activator.CreateInstance(type);
        visited.Add(input, clonedObj);

        foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            if (property.CanWrite)
            {
                var value = property.GetValue(input);
                var propertyType = property.PropertyType;

                if (value != null && propertyType.IsClass && !propertyType.FullName.StartsWith("System."))
                {
                    var clonedValue = CloneObject(value, propertyType, visited);
                    property.SetValue(clonedObj, clonedValue);
                }
                else
                {
                    property.SetValue(clonedObj, value);
                }
            }
        }

        return clonedObj;
    }
}
