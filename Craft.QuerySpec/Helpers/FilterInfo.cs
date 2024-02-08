using System.Linq.Expressions;
using System.Reflection;
using Craft.QuerySpec.Builders;
using Craft.QuerySpec.Enums;

namespace Craft.QuerySpec.Helpers;

public class FilterInfo(string typeName, string name, string value, ComparisonType comparison = ComparisonType.EqualTo)
{
    #region Public Properties

    public ComparisonType Comparison { get; } = comparison;
    public string Name { get; } = name;
    public string TypeName { get; } = typeName;
    public string Value { get; } = value;

    #endregion Public Properties

    #region Private Methods

    private static bool IsValidExpression<T>(Expression<Func<T, bool>> expression)
    {
        return expression.Body is BinaryExpression binary &&
            binary.Left is MemberExpression left &&
            binary.Right is ConstantExpression &&
            left.Member is MemberInfo _;
    }

    private static FilterInfo ParseExpression<T>(Expression<Func<T, bool>> expression)
    {
        var binaryExpression = (BinaryExpression)expression.Body;

        var leftExpression = (MemberExpression)binaryExpression.Left;
        var dataType = leftExpression.Type;
        var comparedValue = ((ConstantExpression)binaryExpression.Right).Value;
        var propertyName = leftExpression.Member.Name;

        var comparisonOperator = binaryExpression.NodeType switch
        {
            ExpressionType.Equal => ComparisonType.EqualTo,
            ExpressionType.NotEqual => ComparisonType.NotEqualTo,
            ExpressionType.GreaterThan => ComparisonType.GreaterThan,
            ExpressionType.LessThan => ComparisonType.LessThan,
            ExpressionType.GreaterThanOrEqual => ComparisonType.GreaterThanOrEqualTo,
            ExpressionType.LessThanOrEqual => ComparisonType.LessThanOrEqualTo,
            _ => throw new ArgumentException("Comparison operator not supported"),
        };

        return new FilterInfo(dataType.FullName,
            propertyName,
            comparedValue.ToString(),
            comparisonOperator);
    }

    #endregion Private Methods

    #region Public Methods

    public static Expression<Func<T, bool>> GetExpression<T>(FilterInfo filterInfo)
        => ExpressionBuilder.CreateWhereExpression<T>(filterInfo);

    public static FilterInfo GetFilterInfo<T>(Expression<Func<T, object>> propName, object compareWith, ComparisonType comparisonType)
    {
        MemberInfo prop = propName.GetPropertyInfo<T>()
            ?? throw new ArgumentException("You must pass a lambda of the form: '() => Class.Property' ");

        string name = prop.Name;
        Type type = prop.GetMemberUnderlyingType();

        if (type.IsEnum)
        {
            type = typeof(int);
            compareWith = (int)compareWith;
        }

        if (Nullable.GetUnderlyingType(type) != null)
            type = type.GetNonNullableType();

        return new FilterInfo(type.FullName, name, compareWith.ToString(), comparisonType);
    }

    public static FilterInfo GetFilterInfo<T>(Expression<Func<T, bool>> whereExpr)
    {
        if (IsValidExpression(whereExpr))
            return ParseExpression(whereExpr);
        else
            throw new ArgumentException("Invalid expression format.");
    }

    public Expression<Func<T, bool>> GetExpression<T>()
        => ExpressionBuilder.CreateWhereExpression<T>(this);

    #endregion Public Methods
}
