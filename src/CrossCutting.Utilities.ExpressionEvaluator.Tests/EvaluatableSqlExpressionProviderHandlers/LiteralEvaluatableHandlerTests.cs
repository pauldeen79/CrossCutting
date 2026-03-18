namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.EvaluatableSqlExpressionProviderHandlers;

public class LiteralEvaluatableHandlerTests : TestBase<LiteralEvaluatableHandler>
{
    public class GetExpressionAsync : LiteralEvaluatableHandlerTests
    {
        [Fact]
        public async Task Returns_Correct_Sql()
        {
            // Arrange
            var parameterBag = new ParameterBag();
            var evaluatable = new LiteralEvaluatableBuilder()
                .WithValue("Hello world!")
                .Build();
            var sut = CreateSut();
            var callback = new EvaluatableSqlExpressionProvider([]);

            // Act
            var result = await sut.GetExpressionAsync(null, evaluatable, Substitute.For<IFieldNameProvider>(), parameterBag, callback, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe("@p0");
            parameterBag.Parameters.Count.ShouldBe(1);
            parameterBag.Parameters.First().Value.ShouldBeEquivalentTo("Hello world!");
        }
    }
}