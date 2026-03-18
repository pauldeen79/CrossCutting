namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.EvaluatableSqlExpressionProviderHandlers;

public class EmptyEvaluatableHandlerTests : TestBase<EmptyEvaluatableHandler>
{
    public class GetExpressionAsync : EmptyEvaluatableHandlerTests
    {
        [Fact]
        public async Task Returns_Correct_Sql()
        {
            // Arrange
            var parameterBag = new ParameterBag();
            var evaluatable = new EmptyEvaluatable();
            var sut = CreateSut();
            var callback = new EvaluatableSqlExpressionProvider([]);

            // Act
            var result = await sut.GetExpressionAsync(null, evaluatable, Substitute.For<IFieldNameProvider>(), parameterBag, callback, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBeEmpty();
            parameterBag.Parameters.Count.ShouldBe(0);
        }
    }
}