using System.Linq.Expressions;
using System.Reflection;
using Craft.Extensions.System;

namespace System.Linq;

public static class LinqExtensions
{
    /// <summary>
    /// Extension method for IQueryable<T> to dynamically apply sorting based on a comma-separated list of property names and optional ASC or DESC indicators.
    /// </summary>
    /// <typeparam name="T">Type of elements in the IQueryable.</typeparam>
    /// <param name="source">The IQueryable to apply sorting to.</param>
    /// <param name="orderList">Comma-separated list of property names for sorting, with optional ASC or DESC indicators.</param>
    /// <returns>New IQueryable<T> with the specified sorting applied.</returns>
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderList)
    {
        // Check for null source or orderList
        if (source is null || orderList is null) return source;

        // Initial expression based on the source
        Expression queryExpr = source.Expression;

        // Method names for ascending and descending order
        string methodAsc = "OrderBy";
        string methodDesc = "OrderByDescending";

        // Remove The PostFix Comma If It Is There By Mistake
        orderList = orderList.RemovePostFix([","])!;

        // Separate All The Order Items In A List
        List<string> orderItems = orderList
            .Trim()
            .Split(',')
            .Select(x => x.Trim())
            .ToList();

        // Get The Entity
        Type entity = typeof(T);

        // Left parameter for lambda expressions
        ParameterExpression parameter = Expression.Parameter(entity, "p");

        // Process Order Items One By One
        foreach (string orderItem in orderItems)
        {
            // Determine ascending or descending
            string command = orderItem.EndsWith("DESC", StringComparison.OrdinalIgnoreCase)
                ? methodDesc
                : methodAsc;

            // Get propertyname and remove optional ASC or DESC
            string propertyName = orderItem.Split(' ')[0].Trim();

            // Get The Ordering Property
            PropertyInfo property = entity.GetProperty(propertyName)!;

            // Get The Property Access, i.e., The Right Parameter
            MemberExpression propertyAccess = Expression.MakeMemberAccess(parameter, property);

            // Create The Lambada Expression
            LambdaExpression orderByExpression = Expression.Lambda(propertyAccess, parameter);

            // Add THIS Order By Call To Expression
            queryExpr = Expression.Call(
                typeof(Queryable), command, [entity, property.PropertyType],
                queryExpr, Expression.Quote(orderByExpression));

            // Set These Values For The Next Order Item
            methodAsc = "ThenBy";
            methodDesc = "ThenByDescending";
        }

        // Finally, Return The Result
        return source.Provider.CreateQuery<T>(queryExpr);
    }

    /// <summary>
    /// Orders an IQueryable<T> sequence in ascending or descending order based on a specified property name.
    /// Supports dynamic sorting using string property names.
    /// </summary>
    /// <typeparam name="T">The type of elements in the source queryable.</typeparam>
    /// <param name="source">The source queryable to order.</param>
    /// <param name="propertyName">The name of the property to order by.</param>
    /// <param name="isDescending">Whether to order in descending order (default is ascending).</param>
    /// <returns>An IQueryable<T> representing the ordered queryable.</returns>
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool isDescending = false)
    {
        if (source is null || propertyName is null) return source;

        var propertyInfo = typeof(T).GetProperty(propertyName);
        var parameter = Expression.Parameter(typeof(T), "p");
        var property = Expression.MakeMemberAccess(parameter, propertyInfo);
        var lambda = Expression.Lambda(property, parameter);

        var methodName = isDescending ? "OrderByDescending" : "OrderBy";
        var resultExpression = Expression.Call(typeof(Queryable), methodName, [typeof(T), propertyInfo.PropertyType], source.Expression, Expression.Quote(lambda));

        return source.Provider.CreateQuery<T>(resultExpression);
    }
}
