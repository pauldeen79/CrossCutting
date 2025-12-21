namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.Expressions;

public class OtherExpressionTests : TestBase
{
    public class EvaluateAsync : OtherExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var evaluator = Substitute.For<IExpressionEvaluator>();
            var context = CreateContext("dummy expression", evaluator: evaluator);
            var sut = new OtherExpression("expression");
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

    public class ParseAsync : OtherExpressionTests
    {
        [Fact]
        public async Task Returns_Correct_Result()
        {
            // Arrange
            var evaluator = Substitute.For<IExpressionEvaluator>();
            var context = CreateContext("dummy expression", evaluator: evaluator);
            var sut = new OtherExpression("expression");
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

    public class ToBuilder : OtherExpressionTests
    {
        [Fact]
        public void Can_Use_ToBuilder_To_Alter_Values_And_Create_New_Expression()
        {
            // Arrange
            var sut = new OtherExpression("expression");

            // Act
            var builder = sut.ToBuilder();
            var sut2 = builder.Build();

            // Assert
            sut2.ShouldBeOfType<OtherExpression>();
            ((OtherExpression)sut2).SourceExpression.ShouldBe(sut.SourceExpression);
        }
    }
}
