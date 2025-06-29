namespace CrossCutting.Utilities.QueryEvaluator.Tests.Operators;

public class EqualsOperatorTests : TestBase<EqualsOperator>
{
    public class Evaluate : EqualsOperatorTests
    {
        [Fact]
        public void Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "THIS";
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
            result.Value.ShouldBe(false);
        }
    }
}
