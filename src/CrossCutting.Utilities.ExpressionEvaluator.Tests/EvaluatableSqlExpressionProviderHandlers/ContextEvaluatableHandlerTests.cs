namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.EvaluatableSqlExpressionProviderHandlers;

public class ContextEvaluatableHandlerTests : TestBase<ContextEvaluatableHandler>
{
    public class GetExpressionAsync : ContextEvaluatableHandlerTests
    {
        [Fact]
        public async Task Returns_Correct_Sql()
        {
            // Arrange
            var parameterBag = new ParameterBag();
            var evaluatable = new ContextEvaluatableBuilder().Build();
            var sut = CreateSut();
            var callback = new EvaluatableSqlExpressionProvider([]);

            // Act
            var result = await sut.GetExpressionAsync("Context", evaluatable, Substitute.For<IFieldNameProvider>(), parameterBag, callback, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("@p0");
            parameterBag.Parameters.Count.ShouldBe(1);
            parameterBag.Parameters.First().Value.ShouldBeEquivalentTo("Context");
        }
    }
}