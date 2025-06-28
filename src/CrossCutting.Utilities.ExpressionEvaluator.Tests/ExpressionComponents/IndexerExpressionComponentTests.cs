namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class IndexerExpressionComponentTests : TestBase<IndexerExpressionComponent>
{
    public class EvaluateAsync : IndexerExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Continue_On_Non_Matching_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("Some expression without indexer");

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Ok_On_Matching_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext($"{Constants.State}[1]", state: new object[] { 1, 2, 3 });

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(2);
        }
    }

    public class ParseAsync : IndexerExpressionComponentTests
    {
        [Fact]
        public async Task Returns_Continue_On_Non_Matching_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("Some expression without indexer");

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Ok_On_Matching_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext($"{Constants.State}[1]", state: new int[] { 1, 2, 3 });

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(int));
        }

        [Fact]
        public async Task Returns_Error_On_Matching_Expression_When_Parse_Result_Is_Not_Successful()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext($"{Constants.State}[666]", state: new int[] { 1, 2, 3 });

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }
}
