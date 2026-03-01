namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.EvaluatableSqlExpressionProviderHandlers;

public class GreaterOrEqualOperatorEvaluatableHandlerTests : TestBase<GreaterOrEqualOperatorEvaluatableHandler>
{
    public class GetExpressionAsync : GreaterOrEqualOperatorEvaluatableHandlerTests
    {
        [Fact]
        public async Task Returns_Correct_Sql()
        {
            // Arrange
            var parameterBag = new ParameterBag();
            var evaluatable = new GreaterOrEqualOperatorEvaluatableBuilder()
                .WithLeftOperand(new LiteralEvaluatableBuilder())
                .WithRightOperand(new LiteralEvaluatableBuilder())
                .Build();
            var sut = CreateSut();
            var callback = new EvaluatableSqlExpressionProvider([new LiteralEvaluatableHandler()]);

            // Act
            var result = await sut.GetExpressionAsync(null, evaluatable, Substitute.For<IFieldNameProvider>(), parameterBag, callback, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("@p0 >= @p1");
        }
    }
}