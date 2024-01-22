using System.Linq.Expressions;
using Craft.QuerySpec.Builders;
using FluentAssertions;

namespace Craft.QuerySpec.Tests.Builders;

public class TestClass
{
    public string Id { get; set; }
    public string StringValue { get; set; }
    public int NumericValue { get; set; }
    public IEnumerable<string> Collection { get; set; }
}

public class ToBinaryTree_EmptyOrNullOrIncorrectFilter_ReturnsNull_DATA : TheoryData<IDictionary<string, string>>
{
    public ToBinaryTree_EmptyOrNullOrIncorrectFilter_ReturnsNull_DATA()
    {
        Add(null);
        Add(new Dictionary<string, string>());
        Add(new Dictionary<string, string> { { nameof(TestClass.NumericValue), "d" } });
    }
}

public class ExpressionTreeBuilderTests
{
    [Theory]
    [InlineData("idsfdsadffffdsfsdf2")] //no equality
    [InlineData("id 2")] // no equality
    [InlineData("id == == 2")] // duplicate equlity
    [InlineData("id == != 2")] // duplicate equlity
    [InlineData("id == <= 2")] // duplicate equlity
    [InlineData("id == < 2")] // duplicate equlity
    [InlineData("id == >= 2")] // duplicate equlity
    [InlineData("id == > 2")] // duplicate equlity
    [InlineData("id != > 2")] // duplicate equlity
    [InlineData("id == 2  value1 ==32")]// missing evaluation
    [InlineData("(id == 2 || value1 ==32) value2 <123")] // missing evaluation
    [InlineData("(id == 2  value1 ==32)")] // missing evaluation
    [InlineData("(id == 2 || value1 ==32) value2 <123 || claue3 = 9898")] // missing evaluation
    [InlineData("id == 2 (value1 ==32 && value2 <123)")] // missing evaluation
    [InlineData("\"this-is-id\" == id")] // property name is second
    public void ToBinaryTree_FromString_ReturnsNull(string query)
    {
        ExpressionTreeBuilder.BuildBinaryTreeExpression(typeof(TestClass), query).Should().BeNull();
    }

    [Theory]
    [InlineData("id == \"this-is-id\"", "x => (x.Id == \"this-is-id\")")]
    [InlineData("id == 2", "x => (x.Id == \"2\")")]
    [InlineData("(id == 2)", "x => (x.Id == \"2\")")]
    [InlineData("NumericValue > 2", "x => (x.NumericValue > 2)")]
    [InlineData("NumericValue >= 3", "x => (x.NumericValue >= 3)")]
    [InlineData("NumericValue < 3", "x => (x.NumericValue < 3)")]
    [InlineData("NumericValue <= 3", "x => (x.NumericValue <= 3)")]
    [InlineData("id == 2 & numericValue ==32", "x => ((x.Id == \"2\") And (x.NumericValue == 32))")]
    [InlineData("id == 2 && numericValue ==32", "x => ((x.Id == \"2\") AndAlso (x.NumericValue == 32))")]
    [InlineData("id == 2 && numericValue ==32 & stringValue==a", "x => ((x.Id == \"2\") AndAlso ((x.NumericValue == 32) And (x.StringValue == \"a\")))")]
    [InlineData("id == 2 & numericValue ==32 & stringValue==a", "x => ((x.Id == \"2\") And ((x.NumericValue == 32) And (x.StringValue == \"a\")))")]
    [InlineData("id == 2 && numericValue ==32 && stringValue==a", "x => ((x.Id == \"2\") AndAlso ((x.NumericValue == 32) AndAlso (x.StringValue == \"a\")))")]
    [InlineData("id == 2 | numericValue ==32", "x => ((x.Id == \"2\") Or (x.NumericValue == 32))")]
    [InlineData("id == 2 || numericValue ==32", "x => ((x.Id == \"2\") OrElse (x.NumericValue == 32))")]
    [InlineData("id == 2 || numericValue ==32 || stringValue==a", "x => ((x.Id == \"2\") OrElse ((x.NumericValue == 32) OrElse (x.StringValue == \"a\")))")]
    [InlineData("id == 2 || numericValue ==32 |  stringValue==a", "x => ((x.Id == \"2\") OrElse ((x.NumericValue == 32) Or (x.StringValue == \"a\")))")]
    [InlineData("(id == 2 || numericvalue <3 && stringValue ==a)", "x => ((x.Id == \"2\") OrElse ((x.NumericValue < 3) AndAlso (x.StringValue == \"a\")))")]
    [InlineData("(id == 2 || numericvalue <3) && stringValue ==a", "x => (((x.Id == \"2\") OrElse (x.NumericValue < 3)) AndAlso (x.StringValue == \"a\"))")]
    [InlineData("id == 2 || (numericvalue ==32 && stringValue ==a)", "x => ((x.Id == \"2\") OrElse ((x.NumericValue == 32) AndAlso (x.StringValue == \"a\")))")]
    [InlineData("id == 2 || (numericvalue ==32 && stringValue ==a) || stringValue ==b", "x => ((x.Id == \"2\") OrElse (((x.NumericValue == 32) AndAlso (x.StringValue == \"a\")) OrElse (x.StringValue == \"b\")))")]
    public void ToBinaryTree_FromString(string query, string expResult)
    {
        var e = ExpressionTreeBuilder.BuildBinaryTreeExpression(typeof(TestClass), query);

        e.ToString().Should().Be(expResult);
    }

    [Theory]
    [InlineData("==")]
    [InlineData("!=")]
    [InlineData(">")]
    [InlineData(">=")]
    [InlineData("<")]
    [InlineData("<=")]
    public void BinaryExpressionBuilder_Keys_returnBuilder(string key)
    {
        ExpressionBuilderForTest.GetBinaryExpressionBuilder(key).Should().NotBeNull();
    }

    [Theory]
    [InlineData("&")]
    [InlineData("&&")]
    [InlineData("|")]
    [InlineData("||")]
    public void EvaluationExpressionBuilder_Keys_returnBuilder(string key)
    {
        ExpressionBuilderForTest.GetEvaluationExpressionBuilder(key).Should().NotBeNull();
    }

    [Fact]
    public void AllRegExPatterns()
    {
        ExpressionBuilderForTest.HasBracketValue.Should().Be(@"^\s*(?'leftOperand'[^\|\&]*)\s*(?'evaluator_first'((\|)*|(\&)*))\s*(?'brackets'(\(\s*(.*)s*\)))\s*(?'evaluator_second'((\|{1,2})|(\&{1,2}))*)\s*(?'rightOperand'.*)\s*$");
        ExpressionBuilderForTest.HasSurroundingBracketsOnlyValue.Should().Be(@"^\s*\(\s*(?'leftOperand'([^\(\)])+)\s*\)\s*$");
        ExpressionBuilderForTest.EvalPatternValue.Should().Be(@"^(?'leftOperand'\S{1,}\s*(==|!=|<|<=|>|>=)\s*\S{1,})\s*(?'evaluator_first'((\|{1,2})|(\&{1,2})))\s*(?'rightOperand'.*)\s*$");
        ExpressionBuilderForTest.BinaryPatternValue.Should().Be(@"^\s*(?'leftOperand'\w+)\s*(?'operator'(==|!=|<|<=|>|>=))\s*(?'rightOperand'\w+)\s*$");
        ExpressionBuilderForTest.EscapedBinaryPatternValue.Should().Be(@"^\s*(\""\s*(?'leftOperand'.*)\s*\""\s*(?'operator'(==|!=|<|<=|>|>=))\s*(?'rightOperand'\w+)|(?'leftOperand'\w+)\s*(?'operator'(==|!=|<|<=|>|>=))\s*\""\s*(?'rightOperand'.*)\s*\""\s*)\s*$");
        ExpressionBuilderForTest.BinaryWithBracketsPatternValue.Should().Be(@"^\s*\(\s*(?'leftOperand'\w+)\s*(?'operator'(==|!=|<|<=|>|>=))\s*(?'rightOperand'\w+)\s*\)\s*$");
    }

    [Theory]
    [ClassData(typeof(ToBinaryTree_EmptyOrNullOrIncorrectFilter_ReturnsNull_DATA))]
    //[MemberData(nameof(ToBinaryTree_EmptyOrNullOrIncorrectFilter_ReturnsNull_DATA))]
    public void ToBinaryTree_EmptyOrNullFilter_ReturnsNull(IDictionary<string, string> filter)
    {
        ExpressionTreeBuilder.BuildBinaryTreeExpression<TestClass>(filter).Should().BeNull();
    }

    [Fact]
    public void ToBinaryTree_PropertyNotExists()
    {
        var filter = new Dictionary<string, string> { { "p", "d" } };
        ExpressionTreeBuilder.BuildBinaryTreeExpression<TestClass>(filter).Should().BeNull();
    }

    [Fact]
    public void ToBinaryTree_BuildsExpression()
    {
        var col = new[]
        {
                new TestClass{Id = "1", NumericValue  =1, StringValue = "1"},
                new TestClass{Id = "2", NumericValue  =1, StringValue = "2"},
                new TestClass{Id = "3", NumericValue  =3},
            };

        var filter1 = new Dictionary<string, string> { { nameof(TestClass.NumericValue), "1" } };
        var f1 = ExpressionTreeBuilder.BuildBinaryTreeExpression<TestClass>(filter1);
        f1.Should().NotBeNull();
        var res1 = col.Where(f1).ToArray();
        res1.Length.Should().Be(2);

        var filter2 = new Dictionary<string, string> {
                {nameof(TestClass.StringValue), "1" } ,
                {nameof(TestClass.NumericValue), "1" } ,
                };
        var f2 = ExpressionTreeBuilder.BuildBinaryTreeExpression<TestClass>(filter2);
        f2.Should().NotBeNull();
        var res2 = col.Where(f2).ToArray();
        res2.Length.Should().Be(1);
    }
}

internal class ExpressionBuilderForTest : ExpressionTreeBuilder
{
    internal static Func<MemberExpression, object, Expression> GetBinaryExpressionBuilder(string key) => BinaryExpressionBuilder[key];
    internal static Func<Expression, Expression, Expression> GetEvaluationExpressionBuilder(string key) => EvaluationExpressionBuilder[key];
    internal const string EvalPatternValue = EvalPattern;
    internal const string BinaryPatternValue = BinaryPattern;
    internal const string EscapedBinaryPatternValue = EscapedBinaryPattern;
    internal const string BinaryWithBracketsPatternValue = BinaryWithBracketsPattern;
    internal const string HasBracketValue = HasBrackets;
    internal const string HasSurroundingBracketsOnlyValue = HasSurroundingBracketsOnly;
}
