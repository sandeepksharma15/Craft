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
    public static Expression<Func<T, object>> CreateMemberExpression<T>(this string propertyName)
    {
        ArgumentException.ThrowIfNullOrEmpty(propertyName, nameof(propertyName));

        try
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var property = Expression.Property(parameter, propertyName);
            UnaryExpression convertedExpression = Expression.Convert(property, typeof(object));

            return Expression.Lambda<Func<T, object>>(convertedExpression, parameter);
        }
        catch (NullReferenceException)
        {
            throw new ArgumentException($"Property {propertyName} not found on type {nameof(T)}");
        }
    }

    public static MemberInfo GetPropertyMemberInfo<T>(this Expression<Func<T, object>> expression)
    {
        if (expression == null) return null;

        try
        {
            if (expression.Body is not MemberExpression body)
            {
                UnaryExpression ubody = (UnaryExpression)expression.Body;
                body = ubody.Operand as MemberExpression;
            }

            return body.Member;
        }
        catch { return null; }
    }
}
