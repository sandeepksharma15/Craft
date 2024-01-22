using System.Linq.Expressions;
using System.Reflection;

namespace Craft.Extensions.Expressions;

public static class ExpressionExtensions
{
    /// <summary>
    /// Creates a member expression for the specified property name of a given type.
    /// The resulting expression is a conversion to object to allow representing properties of various types uniformly.
    /// </summary>
    /// <typeparam name="T">The type containing the property.</typeparam>
    /// <param name="propertyName">The name of the property.</param>
    /// <returns>An expression representing the specified property.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the property name is null or empty, or if the property is not found on the type.
    /// </exception>
    public static LambdaExpression CreateMemberExpression<T>(this string propertyName)
    {
        ArgumentException.ThrowIfNullOrEmpty(propertyName, nameof(propertyName));

        try
        {
            // Create a parameter expression for the lambda
            var parameter = Expression.Parameter(typeof(T), "x");

            // Create a member expression for accessing the specified member
            MemberExpression memberExpression = Expression.PropertyOrField(parameter, propertyName);

            // Create and return the LambdaExpression
            return Expression.Lambda(memberExpression, parameter);
        }
        catch (NullReferenceException)
        {
            throw new ArgumentException($"Property {propertyName} not found on type {nameof(T)}");
        }
    }

    /// <summary>
    /// Creates a LambdaExpression for accessing a specified member of the given Type.
    /// </summary>
    /// <param name="type">The Type containing the member.</param>
    /// <param name="memberName">The name of the member to access.</param>
    /// <returns>
    /// A LambdaExpression representing the access to the specified member.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the specified member is not found on the given Type.
    /// </exception>
    public static LambdaExpression CreateMemberExpression(this Type type, string memberName)
    {
        // Ensure the specified member exists on the given Type
        _ = type.GetMemberByName(memberName)
            ?? throw new ArgumentException($"Property {memberName} not found on type {type}");

        // Create a parameter expression for the lambda
        ParameterExpression parameter = Expression.Parameter(type, "x");

        // Create a member expression for accessing the specified member
        MemberExpression memberExpression = Expression.PropertyOrField(parameter, memberName);

        // Create and return the LambdaExpression
        return Expression.Lambda(memberExpression, parameter);
    }
}
