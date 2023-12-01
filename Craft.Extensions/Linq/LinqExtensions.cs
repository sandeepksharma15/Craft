using System.Linq.Expressions;
using System.Reflection;
using Craft.Extensions.System;

namespace System.Linq;

public static class LinqExtensions
{
    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string orderList)
    {
        if (source is null || orderList is  null) return source;

        Expression queryExpr = source.Expression;

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

        // That Is Our Left Parameter
        ParameterExpression parameter = Expression.Parameter(entity, "p");

        // Process Order Items One By One
        foreach (string orderItem in orderItems)
        {
            // Do We Need Ascending Or Descending
            string command = orderItem.EndsWith("DESC", StringComparison.OrdinalIgnoreCase)
                ? methodDesc
                : methodAsc;

            //Get propertyname and remove optional ASC or DESC
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

    public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string propertyName, bool isDescending = false)
    {
        if (source is null || propertyName is null) return source;

        var propertyInfo = typeof(T).GetPropertyInfo(propertyName);
        var parameter = Expression.Parameter(typeof(T), "p");
        var property = Expression.Property(parameter, propertyInfo);
        var lambda = Expression.Lambda(property, parameter);

        var methodName = isDescending ? "OrderByDescending" : "OrderBy";
        var resultExpression = Expression.Call(typeof(Queryable), methodName, [typeof(T), propertyInfo.PropertyType], source.Expression, Expression.Quote(lambda));

        return source.Provider.CreateQuery<T>(resultExpression);
    }
}
