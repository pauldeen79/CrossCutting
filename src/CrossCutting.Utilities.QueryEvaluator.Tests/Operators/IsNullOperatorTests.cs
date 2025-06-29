namespace CrossCutting.Utilities.QueryEvaluator.Tests.Operators;

public class IsNullOperatorTests : TestBase<IsNullOperator>
{
    public class Evaluate : IsNullOperatorTests
    {
        [Fact]
        public void Returns_Ok_On_Non_Null_Value()
        {
            // Arrange
            var leftValue = "non null value";
            var rightValue = default(object?);
            StringComparison = StringComparison.OrdinalIgnoreCase;
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(leftValue, rightValue);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(false);
        }

        [Fact]
        public void Returns_Ok_On_Null_value()
        {
            // Arrange
            var leftValue = default(object?);
            var rightValue = default(object?);
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
