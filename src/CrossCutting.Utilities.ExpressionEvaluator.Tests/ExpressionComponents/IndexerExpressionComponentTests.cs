namespace CrossCutting.Utilities.ExpressionEvaluator.Tests.ExpressionComponents;

public class IndexerExpressionComponentTests : TestBase<IndexerExpressionComponent>
{
    public class Evaluate : IndexerExpressionComponentTests
    {
        [Fact]
        public void Returns_Continue_On_Non_Matching_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("Some expression without indexer");

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Ok_On_Matching_Expression()
        {
            // Arrange
            var sut = CreateSut();
            // Need a little hack here for EvaluateTyped...
            Evaluator
                .EvaluateTyped<IEnumerable>(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(Result.Success<IEnumerable>(new object[] { 1, 2, 3 }));
            Evaluator
                .EvaluateTyped<int>(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(Result.Success(1));
            var context = CreateContext("state[1]", state: new object[] { 1, 2, 3 });

            // Act
            var result = sut.Evaluate(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(2);
        }
    }

    public class Parse : IndexerExpressionComponentTests
    {
        [Fact]
        public void Returns_Continue_On_Non_Matching_Expression()
        {
            // Arrange
            var sut = CreateSut();
            var context = CreateContext("Some expression without indexer");

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Continue);
        }

        [Fact]
        public void Returns_Ok_On_Matching_Expression()
        {
            // Arrange
            var sut = CreateSut();
            // Need a little hack here for EvaluateTyped...
            Evaluator
                .EvaluateTyped<IEnumerable>(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(Result.Success<IEnumerable>(new object[] { 1, 2, 3 }));
            Evaluator
                .EvaluateTyped<int>(Arg.Any<ExpressionEvaluatorContext>())
                .Returns(Result.Success(1));
            var context = CreateContext("state[1]", state: new object[] { 1, 2, 3 });

            // Act
            var result = sut.Parse(context);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.ResultType.ShouldBe(typeof(int));
        }
    }
}
