namespace CrossCutting.Utilities.QueryEvaluator.Tests.Operators;

public class NotEndsWithOperatorTests : TestBase<NotEndsWithOperator>
{
    public class Evaluate : NotEndsWithOperatorTests
    {
        [Fact]
        public void Returns_Ok_On_Two_Strings()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = "t";
            StringComparison = StringComparison.OrdinalIgnoreCase;
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(leftValue, rightValue);

            // Assert
            result.Status.ShouldBe(ResultStatus.Ok);
            result.Value.ShouldBe(true);
        }

        [Fact]
        public void Returns_Invalid_On_Left_No_String()
        {
            // Arrange
            var leftValue = 1;
            var rightValue = "this";
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(leftValue, rightValue);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("LeftValue and RightValue both need to be of type string");
        }

        [Fact]
        public void Returns_Invalid_On_Right_No_String()
        {
            // Arrange
            var leftValue = "this";
            var rightValue = 2;
            var sut = CreateSut();

            // Act
            var result = sut.Evaluate(leftValue, rightValue);

            // Assert
            result.Status.ShouldBe(ResultStatus.Invalid);
            result.ErrorMessage.ShouldBe("LeftValue and RightValue both need to be of type string");
        }
    }
}
