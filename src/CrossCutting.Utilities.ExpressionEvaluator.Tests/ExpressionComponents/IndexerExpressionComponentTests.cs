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
            var result = await sut.EvaluateAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Ok_On_Matching_Expression()
        {
            // Arrange
            var sut = CreateSut();
            // Need a little hack here for EvaluateTyped...
            Evaluator
                .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(x => x.ArgAt<ExpressionEvaluatorContext>(0).Expression == "state"
                    ? Result.Success<object?>(new object[] { 1, 2, 3 })
                    : Result.Success<object?>(1));
            var context = CreateContext("state[1]", state: new object[] { 1, 2, 3 });

            // Act
            var result = await sut.EvaluateAsync(context);

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
            var result = await sut.ParseAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public async Task Returns_Ok_On_Matching_Expression()
        {
            // Arrange
            var sut = CreateSut();
            // Need a little hack here for EvaluateTyped...
            Evaluator
                .EvaluateAsync(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(x => x.ArgAt<ExpressionEvaluatorContext>(0).Expression == "state"
                    ? Result.Success<object?>(new object[] { 1, 2, 3 })
                    : Result.Success<object?>(1));
            var context = CreateContext("state[1]", state: new int[] { 1, 2, 3 });

            // Act
            var result = await sut.ParseAsync(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(int));
        }
    }
}
