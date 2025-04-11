namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Operators;

public class ExpressionOperatorTests : TestBase
{
    public class Evaluate : ExpressionOperatorTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ExpressionOperator("expression");
            var evaluator = Substitute.For<IExpressionEvaluator>();
            evaluator
                .Evaluate(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(Result.Success<object?>("the result"));

            // Act
            var result = sut.Evaluate(CreateContext("dummy expression", evaluator: evaluator));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("the result");
        }
    }

    public class Parse : ExpressionOperatorTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new ExpressionOperator("expression");
            var evaluator = Substitute.For<IExpressionEvaluator>();
            evaluator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithSourceExpression("expression").WithExpressionType(GetType()).WithResultType(typeof(string)));

            // Act
            var result = sut.Parse(CreateContext("dummy expression", evaluator: evaluator));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionType.ShouldBe(typeof(OperatorExpression));
            result.ResultType.ShouldBe(typeof(string));
        }
    }
}
