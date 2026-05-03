namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.EvaluatableSqlExpressionProviderHandlers;

public class SmallerOrEqualOperatorEvaluatableHandlerTests : TestBase<SmallerOrEqualOperatorEvaluatableHandler>
{
    public class GetExpressionAsync : SmallerOrEqualOperatorEvaluatableHandlerTests
    {
        [Fact]
        public async Task Returns_Correct_Sql()
        {
            // Arrange
            var parameterBag = new ParameterBag();
            var evaluatable = new SmallerOrEqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder())
                .WithRightOperand(new LiteralEvaluatableBuilder())
                .Build();
            var sut = CreateSut();
            var callback = new EvaluatableSqlExpressionProvider([new LiteralEvaluatableHandler()]);

            // Act
            var result = await sut.GetExpressionAsync(null, evaluatable, Substitute.For<IFieldNameProvider>(), parameterBag, callback, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("@p0 <= @p1");
        }

        [Fact]
        public async Task Returns_Correct_Sql_With_UnaryNegateOperator()
        {
            // Arrange
            var parameterBag = new ParameterBag();
            var evaluatable = new UnaryNegateOperatorEvaluatableBuilder()
                .WithOperand(new SmallerOrEqualOperatorEvaluatableBuilder()
                    .WithLeftOperand(new LiteralEvaluatableBuilder())
                    .WithRightOperand(new LiteralEvaluatableBuilder()))
                .Build();
            var sut = CreateSut();
            var callback = new EvaluatableSqlExpressionProvider([new LiteralEvaluatableHandler()]);

            // Act
            var result = await sut.GetExpressionAsync(null, evaluatable, Substitute.For<IFieldNameProvider>(), parameterBag, callback, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("@p0 > @p1");
        }
    }
}