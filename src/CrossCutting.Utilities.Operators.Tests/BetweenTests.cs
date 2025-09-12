namespace CrossCutting.Utilities.Operators.Tests;

public class BetweenTests
{
    [Fact]
    public void Returns_False_When_SourceValue_Is_Below_Lower_Bound()
    {
        // Arrange
        var sourceValue = 0;
        var lowerBound = 1;
        var upperBound = 3;

        // Act
        var result = Between.Evaluate(sourceValue, lowerBound, upperBound);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(false);
    }

    [Fact]
    public void Returns_True_When_SourceValue_Is_Equal_To_Lower_Bound()
    {
        // Arrange
        var sourceValue = 1;
        var lowerBound = 1;
        var upperBound = 3;

        // Act
        var result = Between.Evaluate(sourceValue, lowerBound, upperBound);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Returns_True_When_SourceValue_Is_Within_Bounds()
    {
        // Arrange
        var sourceValue = 2;
        var lowerBound = 1;
        var upperBound = 3;

        // Act
        var result = Between.Evaluate(sourceValue, lowerBound, upperBound);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Returns_True_When_SourceValue_Is_Equal_To_Upper_Bound()
    {
        // Arrange
        var sourceValue = 3;
        var lowerBound = 1;
        var upperBound = 3;

        // Act
        var result = Between.Evaluate(sourceValue, lowerBound, upperBound);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(true);
    }

    [Fact]
    public void Returns_False_When_SourceValue_Is_Higher_Than_Upper_Bound()
    {
        // Arrange
        var sourceValue = 4;
        var lowerBound = 1;
        var upperBound = 3;

        // Act
        var result = Between.Evaluate(sourceValue, lowerBound, upperBound);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBe(false);
    }
}
