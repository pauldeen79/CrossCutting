namespace CrossCutting.Utilities.Parsers.Tests.MathematicExpressions.Aggregators;

public class BinaryAndAggregatorTests
{
    [Fact]
    public void Aggregate_Returns_Invalid_When_LeftExpression_Is_Not_Boolean()
    {
        // Arrange
        var sut = new BinaryAndAggregator();

        // Act
        var result = sut.Aggregate("not a boolean", true, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Binary and (&) operator can only be used on boolean values");
    }

    [Fact]
    public void Aggregate_Returns_Invalid_When_RightExpression_Is_Not_Boolean()
    {
        // Arrange
        var sut = new BinaryAndAggregator();

        // Act
        var result = sut.Aggregate(false, "not a boolean", CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Binary and (&) operator can only be used on boolean values");
    }
}
