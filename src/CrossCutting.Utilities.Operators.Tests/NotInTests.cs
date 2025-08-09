namespace CrossCutting.Utilities.Operators.Tests;

public class NotInTests
{
    [Fact]
    public void Returns_False_When_LeftValue_Is_Null_And_RightValue_Is_Enumerable_That_Contains_Null()
    {
        // Arrange
        var leftValue = (object?)null;
        var rightValue = new[] { "A", null };

        // Act
        var result = NotIn.Evaluate(leftValue, rightValue, StringComparison.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(false);
    }

    [Fact]
    public void Returns_False_When_LeftValue_Is_Null_And_RightValue_Is_Not_Enumerable_But_Is_Null()
    {
        // Arrange
        var leftValue = (object?)null;
        var rightValue = (object?)null;

        // Act
        var result = NotIn.Evaluate(leftValue, rightValue, StringComparison.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(false);
    }

    [Fact]
    public void Returns_False_When_LeftValue_Is_Part_Of_RightValue()
    {
        // Arrange
        var leftValue = "A";
        var rightValue = new[] { "A", "B" };

        // Act
        var result = NotIn.Evaluate(leftValue, rightValue, StringComparison.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(false);
    }

    [Fact]
    public void Returns_False_When_LeftValue_Is_Equal_To_RightValue()
    {
        // Arrange
        var leftValue = "A";
        var rightValue = "A";

        // Act
        var result = NotIn.Evaluate(leftValue, rightValue, StringComparison.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(false);
    }
}
