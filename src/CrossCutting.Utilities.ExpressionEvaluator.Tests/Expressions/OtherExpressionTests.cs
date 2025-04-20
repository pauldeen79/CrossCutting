namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class OtherExpressionTests : TestBase
{
    public class Evaluate : OtherExpressionTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new OtherExpression("expression");
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

    public class Parse : OtherExpressionTests
    {
        [Fact]
        public void Returns_Correct_Result()
        {
            // Arrange
            var sut = new OtherExpression("expression");
            var evaluator = Substitute.For<IExpressionEvaluator>();
            evaluator
                .Parse(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithSourceExpression("expression").WithExpressionComponentType(GetType()).WithResultType(typeof(string)));

            // Act
            var result = sut.Parse(CreateContext("dummy expression", evaluator: evaluator));

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(GetType());
            result.ResultType.ShouldBe(typeof(string));
        }
    }
}
