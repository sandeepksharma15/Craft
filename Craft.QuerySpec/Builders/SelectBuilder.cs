using System.Linq.Expressions;
using System.Reflection;
using Craft.QuerySpec.Contracts;
using Craft.QuerySpec.Helpers;

namespace Craft.QuerySpec.Builders;

[Serializable]
public class SelectBuilder<T> : SelectBuilder<T, T>, ISelectBuilder<T> where T : class;

[Serializable]
public class SelectBuilder<T, TResult> : ISelectBuilder<T, TResult>
    where T : class
    where TResult : class
{
    private readonly List<SelectInfo<T, TResult>> _selectList;

    public SelectBuilder()
    {
        _selectList = [];
    }

    public long SelectCount => _selectList.Count;

    public SelectBuilder<T, TResult> Add(LambdaExpression assignor, LambdaExpression assignee)
    {
        _selectList.Add(new SelectInfo<T, TResult>(assignor, assignee));
        return this;
    }

    public SelectBuilder<T, TResult> Add(LambdaExpression column)
    {
        _selectList.Add(new SelectInfo<T, TResult>(column));
        return this;
    }

    public SelectBuilder<T, TResult> Add(string assignorPropName)
    {
        _selectList.Add(new SelectInfo<T, TResult>(assignorPropName));
        return this;
    }

    public SelectBuilder<T, TResult> Add(string assignorPropName, string assigneePropName)
    {
        _selectList.Add(new SelectInfo<T, TResult>(assignorPropName, assigneePropName));
        return this;
    }

    public Expression<Func<T, TResult>> Build()
    {
        if (typeof(TResult) == typeof(object))
            return BuildAnnonymousSelect();
        else
            return BuildSelect();
    }

    public void Clear()
    {
        _selectList.Clear();
    }

    private Expression<Func<T, TResult>> BuildAnnonymousSelect()
    {
        var sourceParam = Expression.Parameter(typeof(T), "x");

        var selectExpressions = _selectList.Select(item =>
        {
            var columnInvoke = Expression.Invoke(item.Assignor, sourceParam);

            return Expression.Convert(columnInvoke, typeof(object));
        });

        var selectorBody = Expression.NewArrayInit(typeof(TResult), selectExpressions);

        return Expression.Lambda<Func<T, TResult>>(selectorBody, sourceParam);
    }

    private Expression<Func<T, TResult>> BuildSelect()
    {
        var selectParam = Expression.Parameter(typeof(T));
        var memberBindings = new List<MemberBinding>();

        foreach (var item in _selectList)
        {
            var columnInvoke = Expression.Invoke(item.Assignor, selectParam);
            var propertyInfo = item.Assignee.GetPropertyInfo();
            var propertyType = propertyInfo.PropertyType;

            var convertedValue = Expression.Convert(columnInvoke, propertyType);

            var memberBinding = Expression.Bind(propertyInfo, convertedValue);
            memberBindings.Add(memberBinding);
        }

        var memberInit = Expression.MemberInit(Expression.New(typeof(TResult)), memberBindings);
        var selectorLambda = Expression.Lambda<Func<T, TResult>>(memberInit, selectParam);

        return Expression.Lambda<Func<T, TResult>>(selectorLambda.Body, selectParam);
    }
}
