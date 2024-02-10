using System.Linq.Expressions;
using Craft.QuerySpec.Builders;

namespace Craft.QuerySpec.Contracts;

public interface ISelectBuilder<T, TResult>
    where T : class
    where TResult : class
{
    SelectBuilder<T, TResult> Add(LambdaExpression column, LambdaExpression assignTo);

    SelectBuilder<T, TResult> Add(LambdaExpression column);

    Expression<Func<T, TResult>> Build();

    void Clear();
}

public interface ISelectBuilder<T> : ISelectBuilder<T, T> where T : class;
