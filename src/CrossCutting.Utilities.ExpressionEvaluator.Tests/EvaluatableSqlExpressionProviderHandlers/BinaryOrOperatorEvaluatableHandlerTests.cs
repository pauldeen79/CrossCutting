namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.EvaluatableSqlExpressionProviderHandlers;

public class BinaryOrOperatorEvaluatableHandlerTests : TestBase<BinaryOrOperatorEvaluatableHandler>
{
    public class GetExpressionAsync : BinaryOrOperatorEvaluatableHandlerTests
    {
        [Fact]
        public async Task Returns_Correct_Sql()
        {
            // Arrange
            var parameterBag = new ParameterBag();
            var evaluatable = new BinaryOrOperatorEvaluatableBuilder()
                .WithLeftOperand(new EqualOperatorEvaluatableBuilder().WithLeftOperand(new LiteralEvaluatableBuilder(true)).WithRightOperand(new LiteralEvaluatableBuilder(true)))
                .WithRightOperand(new EqualOperatorEvaluatableBuilder().WithLeftOperand(new LiteralEvaluatableBuilder(false)).WithRightOperand(new LiteralEvaluatableBuilder(false)))
                .Build();
            var sut = CreateSut();
            var callback = new EvaluatableSqlExpressionProvider([new LiteralEvaluatableHandler(), new EqualOperatorEvaluatableHandler()]);

            // Act
            var result = await sut.GetExpressionAsync(null, evaluatable, Substitute.For<IFieldNameProvider>(), parameterBag, callback, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("(@p0 = @p1 OR @p2 = @p3)");
        }
    }
}