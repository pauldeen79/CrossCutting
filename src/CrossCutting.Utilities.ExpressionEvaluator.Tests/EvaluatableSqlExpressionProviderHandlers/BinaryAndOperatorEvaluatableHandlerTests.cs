namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.EvaluatableSqlExpressionProviderHandlers;

public class BinaryAndOperatorEvaluatableHandlerTests : TestBase<BinaryAndOperatorEvaluatableHandler>
{
    public class GetExpressionAsync : BinaryAndOperatorEvaluatableHandlerTests
    {
        [Fact]
        public async Task Returns_Correct_Sql()
        {
            // Arrange
            var parameterBag = new ParameterBag();
            var evaluatable = new LiteralEvaluatableBuilder(true).IsEqualTo(new LiteralEvaluatableBuilder(true))
                .And(new LiteralEvaluatableBuilder(false).IsEqualTo(new LiteralEvaluatableBuilder(false)))
                .Build();
            var sut = CreateSut();
            var callback = new EvaluatableSqlExpressionProvider([new LiteralEvaluatableHandler(), new EqualOperatorEvaluatableHandler()]);

            // Act
            var result = await sut.GetExpressionAsync(null, evaluatable, Substitute.For<IFieldNameProvider>(), parameterBag, callback, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("(@p0 = @p1 AND @p2 = @p3)");
        }
    }
}