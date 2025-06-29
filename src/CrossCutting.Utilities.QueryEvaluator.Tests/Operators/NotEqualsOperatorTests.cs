namespace CrossCutting.Utilities.QueryEvaluator.Tests.Operators;

public class NotEqualsOperatorTests : TestBase<NotEqualsOperator>
{
    public class Evaluate : NotEqualsOperatorTests
    {
        [Fact]
        public void Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "something else";
            StringComparison = StringComparison.OrdinalIgnoreCase;
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(leftValue, rightValue);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }

        [Fact]
        public void Returns_Ok_On_Different_Types()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = 13;
            StringComparison = StringComparison.OrdinalIgnoreCase;
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(leftValue, rightValue);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }
    }
}
