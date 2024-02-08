using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Craft.Extensions.Expressions;

/// <summary>
/// Provides methods to remove a specified condition from an expression.
/// </summary>
public static class ConditionRemover
{
    #region Public Methods

    /// <summary>
    /// Removes the specified condition from the given expression.
    /// </summary>
    /// <typeparam name="T">The type of the parameter in the expression.</typeparam>
    /// <param name="expression">The original expression.</param>
    /// <param name="condition">The condition to remove.</param>
    /// <returns>A new expression with the condition removed, or null if the conditions are equivalent.</returns>
    public static Expression<Func<T, bool>> RemoveCondition<T>(this Expression<Func<T, bool>> expression,
                Expression<Func<T, bool>> condition)
    {
        ArgumentNullException.ThrowIfNull(expression);
        ArgumentNullException.ThrowIfNull(condition);

        if (ConditionRemoverVisitor<T>.IsEquivalentCondition(expression, condition))
            return null;

        var visitor = new ConditionRemoverVisitor<T>(condition);
        var modifiedBody = visitor.Visit(expression.Body);

        return Expression.Lambda<Func<T, bool>>(modifiedBody, expression.Parameters);
    }

    #endregion Public Methods

    #region Private Classes

    /// <summary>
    /// A visitor class responsible for removing conditions from an expression.
    /// </summary>
    private sealed class ConditionRemoverVisitor<T>(Expression<Func<T, bool>> conditionToRemove) : ExpressionVisitor
    {
        #region Private Fields

        private readonly Expression _conditionToRemove = conditionToRemove.Body;

        #endregion Private Fields

        #region Protected Methods

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == ExpressionType.AndAlso)
            {
                if (IsEquivalentCondition(node.Left, _conditionToRemove))
                {
                    return node.Right;
                }
                else if (IsEquivalentCondition(node.Right, _conditionToRemove))
                {
                    return node.Left;
                }
            }

            return base.VisitBinary(node);
        }

        #endregion Protected Methods

        #region Public Methods

        [global::System.Diagnostics.CodeAnalysis.SuppressMessage("Roslynator", "RCS1158:Static member in generic type should use a type parameter", Justification = "<Pending>")]
        public static bool IsEquivalentCondition(Expression expression1, Expression expression2)
        {
            if (expression1.CanReduce) expression1 = expression1.Reduce();
            if (expression2.CanReduce) expression2 = expression2.Reduce();

            var equalityComparer = new ExpressionSemanticEqualityComparer();

            // May need to customize this logic based on specific conditions
            return ExpressionEqualityComparer.Instance.Equals(expression1, expression2)
                || equalityComparer.Equals(expression1, expression2);
        }

        #endregion Public Methods
    }

    #endregion Private Classes
}
