namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class IndexerExpressionComponentTests : TestBase<IndexerExpressionComponent>
{
    public class Evaluate : IndexerExpressionComponentTests
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
            var context = CreateContext("state[1]", state: new object[] { 1, 2, 3 });

            // Act
            var result = await sut.EvaluateAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(2);
        }
    }

    public class Parse : IndexerExpressionComponentTests
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
            var context = CreateContext("state[1]", state: new int[] { 1, 2, 3 });

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
            Evaluator
                .ParseAsync(Arg.Any<ExpressionEvaluatorContext>(), Arg.Any<CancellationToken>())
                .Returns(x =>
                    x.ArgAt<ExpressionEvaluatorContext>(0).Expression switch
                    {
                        "1" => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Error).WithErrorMessage("Kaboom"),
                        "state" => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Ok).WithResultType(x.ArgAt<ExpressionEvaluatorContext>(0).State?.FirstOrDefault().Value?.Result.Value?.GetType()),
                        _ => new ExpressionParseResultBuilder().WithExpressionComponentType(GetType()).WithStatus(ResultStatus.Ok)
                    });
            var context = CreateContext("state[1]", state: new int[] { 1, 2, 3 });

            // Act
            var result = await sut.ParseAsync(context, CancellationToken.None);

            // Assert
            result.Status.ShouldBe(ResultStatus.Error);
            result.ErrorMessage.ShouldBe("Kaboom");
        }
    }
}
