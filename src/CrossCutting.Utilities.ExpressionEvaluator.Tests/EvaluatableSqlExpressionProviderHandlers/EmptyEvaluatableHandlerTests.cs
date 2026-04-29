namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.EvaluatableSqlExpressionProviderHandlers;

public class EmptyEvaluatableHandlerTests : TestBase<EmptyEvaluatableHandler>
{
    public class GetExpressionAsync : EmptyEvaluatableHandlerTests
    {
        [Fact]
        public async Task Returns_Correct_Sql_For_Untyped_EmptyEvaluatable()
        {
            // Arrange
            var parameterBag = new ParameterBag();
            var evaluatable = new EmptyEvaluatableBuilder().BuildTyped();
            var sut = CreateSut();
            var callback = new EvaluatableSqlExpressionProvider([]);

            // Act
            var result = await sut.GetExpressionAsync(null, evaluatable, Substitute.For<IFieldNameProvider>(), parameterBag, callback, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(string.Empty);
            parameterBag.Parameters.Count.ShouldBe(0);
        }
    
        [Fact]
        public async Task Returns_Correct_Sql_For_Typed_EmptyEvaluatable()
        {
            // Arrange
            var parameterBag = new ParameterBag();
            var evaluatable = new EmptyEvaluatableBuilder<string>().BuildTyped();
            var sut = CreateSut();
            var callback = new EvaluatableSqlExpressionProvider([]);

            // Act
            var result = await sut.GetExpressionAsync(null, evaluatable, Substitute.For<IFieldNameProvider>(), parameterBag, callback, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(string.Empty);
            parameterBag.Parameters.Count.ShouldBe(0);
        }
    }
}