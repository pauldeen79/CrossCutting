namespace CrossCutting.Utilities.Parsers.Tests;

public class NumericAggregatorTests
{
    [Fact]
    public void Evaluate_Returns_Invalid_When_Type_Is_Not_Supported()
    {
        // Act
        var result = NumericAggregator.Evaluate("apple", "pear", CultureInfo.InvariantCulture
            , (_, _) => new object()
            , (_, _) => new object()
            , (_, _) => new object()
            , (_, _) => new object()
            , (_, _) => new object()
            , (_, _) => new object()
            , (_, _) => new object());

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Evaluate_Returns_Error_When_Aggregator_Throws()
    {
        // Act
        var result = NumericAggregator.Evaluate(1, 2, CultureInfo.InvariantCulture
            , (_, _) => new object()
            , (_, _) => new object()
            , (_, _) => throw new InvalidOperationException("Kaboom")
            , (_, _) => new object()
            , (_, _) => new object()
            , (_, _) => new object()
            , (_, _) => new object());

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
        result.ErrorMessage.Should().Be("Aggregation failed. Error message: Kaboom");
    }
}
