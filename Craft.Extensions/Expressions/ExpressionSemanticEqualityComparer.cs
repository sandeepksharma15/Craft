using System.Linq.Expressions;

namespace Craft.Extensions.Expressions;

/// <summary>
/// A custom equality comparer for expressions, implementing IEqualityComparer&lt;Expression&gt;.
/// It normalizes binary expressions to handle "==" and "!=" comparison issues.
/// </summary>
public class ExpressionSemanticEqualityComparer : IEqualityComparer<Expression>
{
    public bool Equals(Expression x, Expression y)
    {
        if (x == y) return true;
        if (x == null || y == null) return false;
        if (x.NodeType != y.NodeType) return false;

        return NormalizeExpression(x).ToString() == NormalizeExpression(y).ToString();
    }

    public int GetHashCode(Expression obj)
        => NormalizeExpression(obj).ToString().GetHashCode();

    private static Expression NormalizeExpression(Expression expression)
    {
        return new ExpressionNormalizer().Visit(expression);
    }

    // The private sealed class ExpressionNormalizer is responsible for normalizing binary expressions.
    // It extends ExpressionVisitor to traverse and modify expression trees.
    private sealed class ExpressionNormalizer : ExpressionVisitor
    {
        protected override Expression VisitBinary(BinaryExpression node)
        {
            // Normalize binary expressions to avoid "==" and "!=" comparison issues
            if (node.NodeType == ExpressionType.Equal || node.NodeType == ExpressionType.NotEqual)
            {
                var left = Visit(node.Left);
                var right = Visit(node.Right);

                var expr = node.Right.NodeType == ExpressionType.Constant ? left : right;
                var constant = (ConstantExpression)(node.Right.NodeType == ExpressionType.Constant ? right : left);

                if (constant.Value is bool value)
                    return value ? expr : Expression.Not(expr);

                return Expression.MakeBinary(ExpressionType.Equal, left, right);
            }

            return base.VisitBinary(node);
        }
    }
}
