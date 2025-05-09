namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class OtherExpressionTests : TestBase
{
    public class Evaluate : OtherExpressionTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var evaluator = Substitute.For<IExpressionEvaluator>();
            var context = CreateContext("dummy expression", evaluator: evaluator);
            var sut = new OtherExpression(context, "expression");
            evaluator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(Result.Success<object?>("the result"));

            // Act
            var result = sut.Evaluate();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("the result");
        }
    }

    public class Parse : OtherExpressionTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var evaluator = Substitute.For<IExpressionEvaluator>();
            var context = CreateContext("dummy expression", evaluator: evaluator);
            var sut = new OtherExpression(context, "expression");
            evaluator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithSourceExpression("expression").WithExpressionComponentType(GetType()).WithResultType(typeof(string)));

            // Act
            var result = sut.Parse();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(GetType());
            result.ResultType.ShouldBe(typeof(string));
        }
    }
}
