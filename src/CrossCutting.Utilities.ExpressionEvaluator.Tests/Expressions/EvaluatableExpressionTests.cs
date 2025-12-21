using CrossCutting.Utilities.ExpressionEvaluator.Builders.Expressions;

namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class EvaluatableExpressionTests : TestBase
{
    public class EvaluateAsync : EvaluatableExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var evaluator = Substitute.For<IExpressionEvaluator>();
            var context = CreateContext("dummy expression", evaluator: evaluator);
            var sut = new EvaluatableExpression("expression");
            evaluator
                .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(Result.Success<object?>("the result"));

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("the result");
        }
    }

    public class ParseAsync : EvaluatableExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var evaluator = Substitute.For<IExpressionEvaluator>();
            var context = CreateContext("dummy expression", evaluator: evaluator);
            var sut = new EvaluatableExpressionBuilder().WithSourceExpression("expression").BuildTyped();
            evaluator
                .ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(new ExpressionParseResultBuilder().WithSourceExpression("expression").WithExpressionComponentType(GetType()).WithResultType(typeof(string)));

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ExpressionComponentType.ShouldBe(GetType());
            result.ResultType.ShouldBe(typeof(string));
        }
    }

    public class ToBuilder : EvaluatableExpressionTests
    {
        [Fact]
        public void Can_Use_ToBuilder_To_Alter_Values_And_Create_New_Expression()
        {
            // Arrange
            var sut = new EvaluatableExpressionBuilder { SourceExpression = "expression" }.BuildTyped();

            // Act
            var builder = sut.ToBuilder();
            var sut2 = builder.Build();

            // Assert
            sut2.ShouldBeOfType<EvaluatableExpression>();
            ((EvaluatableExpression)sut2).SourceExpression.ShouldBe(sut.SourceExpression);
        }
    }
}
