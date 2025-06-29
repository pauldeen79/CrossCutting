namespace CrossCutting.Utilities.QueryEvaluator.Tests.Operators;

public class GreaterOrEqualThanOperatorTests : TestBase<GreaterOrEqualThanOperator>
{
    public class Evaluate : GreaterOrEqualThanOperatorTests
    {
        [Fact]
        public void Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = 15;
            var rightValue = 13;
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(leftValue, rightValue);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }

        [Fact]
        public void Returns_Invalid_On_Different_Types()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = 13;
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(leftValue, rightValue);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("Object must be of type String.");
        }
    }
}
