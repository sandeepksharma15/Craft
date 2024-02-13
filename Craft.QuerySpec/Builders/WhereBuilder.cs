using System.Linq.Expressions;
using Craft.Extensions.Expressions;
using Craft.QuerySpec.Enums;
using Craft.QuerySpec.Helpers;

namespace Craft.QuerySpec.Builders;

[Serializable]
public class WhereBuilder<T> where T : class
{
    public WhereBuilder() => WhereExpressions = [];

    public List<WhereInfo<T>> WhereExpressions { get; }

    public long Count => WhereExpressions.Count;

    public WhereBuilder<T> Add(Expression<Func<T, bool>> expression)
    {
        ArgumentNullException.ThrowIfNullOrEmpty(nameof(expression));

        if (expression.CanReduce)
            expression = (Expression<Func<T, bool>>)expression.ReduceAndCheck();

        WhereExpressions.Add(new WhereInfo<T>(expression));

        return this;
    }

    public WhereBuilder<T> Add(Expression<Func<T, object>> propExpr, object compareWith, ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        var expression = GetExpression(propExpr, compareWith, comparisonType);

        WhereExpressions.Add(new WhereInfo<T>(expression));

        return this;
    }

    public WhereBuilder<T> Add(string propName, object compareWith, ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        var expression = GetExpression(propName, compareWith, comparisonType);

        WhereExpressions.Add(new WhereInfo<T>(expression));

        return this;
    }

    public WhereBuilder<T> Clear()
    {
        WhereExpressions.Clear();

        return this;
    }

    public WhereBuilder<T> Remove(Expression<Func<T, bool>> expression)
    {
        ArgumentNullException.ThrowIfNull(nameof(expression));

        if (expression.CanReduce)
            expression = (Expression<Func<T, bool>>)expression.ReduceAndCheck();

        var comparer = new ExpressionSemanticEqualityComparer();

        var whereInfo = WhereExpressions.Find(x => comparer.Equals(x.Filter, expression));

        if (whereInfo is not null)
            WhereExpressions.Remove(whereInfo);

        return this;
    }

    public WhereBuilder<T> Remove(Expression<Func<T, object>> propExpr, object compareWith, ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        var expression = GetExpression(propExpr, compareWith, comparisonType);

        Remove(expression);

        return this;
    }

    public WhereBuilder<T> Remove(string propName, object compareWith, ComparisonType comparisonType = ComparisonType.EqualTo)
    {
        var expression = GetExpression(propName, compareWith, comparisonType);

        Remove(expression);

        return this;
    }

    private static Expression<Func<T, bool>> GetExpression(Expression<Func<T, object>> propExpr, object compareWith, ComparisonType comparisonType)
    {
        ArgumentNullException.ThrowIfNull(nameof(propExpr));

        var filterInfo = FilterInfo.GetFilterInfo(propExpr, compareWith, comparisonType);

        return ExpressionBuilder.CreateWhereExpression<T>(filterInfo);
    }

    private static Expression<Func<T, bool>> GetExpression(string propName, object compareWith, ComparisonType comparisonType)
    {
        // Check if the property exists
        var propExpr = ExpressionBuilder.GetPropertyExpression<T>(propName);
        var filterInfo = FilterInfo.GetFilterInfo(propExpr, compareWith, comparisonType);

        return ExpressionBuilder.CreateWhereExpression<T>(filterInfo);
    }
}
