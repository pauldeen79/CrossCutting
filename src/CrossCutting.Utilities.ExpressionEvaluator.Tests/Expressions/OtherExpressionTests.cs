namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class OtherExpressionTests : TestBase
{
    public class Evaluate : OtherExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var evaluator = Substitute.For<IExpressionEvaluator>();
            var context = CreateContext("dummy expression", evaluator: evaluator);
            var sut = new OtherExpression(context, "expression");
            evaluator
                .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(Result.Success<object?>("the result"));

            // Act
            var result = await sut.EvaluateAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("the result");
        }
    }

    public class Parse : OtherExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var evaluator = Substitute.For<IExpressionEvaluator>();
            var context = CreateContext("dummy expression", evaluator: evaluator);
            var sut = new OtherExpression(context, "expression");
            evaluator
                .ParseAsync(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(new ExpressionParseResultBuilder().WithSourceExpression("expression").WithExpressionComponentType(GetType()).WithResultType(typeof(string)));

            // Act
            var result = await sut.ParseAsync();

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(GetType());
            result.ResultType.ShouldBe(typeof(string));
        }
    }
}
