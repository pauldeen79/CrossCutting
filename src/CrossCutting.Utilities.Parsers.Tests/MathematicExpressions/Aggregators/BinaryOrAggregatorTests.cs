namespace CrossCutting.Utilities.Parsers.Tests.MathematicExpressions.Aggregators;

public class BinaryOrAggregatorTests
{
    [Fact]
    public void Aggregate_Returns_Invalid_When_LeftExpression_Is_Not_Boolean()
    {
        // Arrange
        var sut = new BinaryOrAggregator();

        // Act
        var result = sut.Aggregate("not a boolean", true, CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Binary or (|) operator can only be used on boolean values");
    }

    [Fact]
    public void Aggregate_Returns_Invalid_When_RightExpression_Is_Not_Boolean()
    {
        // Arrange
        var sut = new BinaryOrAggregator();

        // Act
        var result = sut.Aggregate(false, "not a boolean", CultureInfo.InvariantCulture);

        // Assert
        result.Status.ShouldBe(ResultStatus.Invalid);
        result.ErrorMessage.ShouldBe("Binary or (|) operator can only be used on boolean values");
    }
}
