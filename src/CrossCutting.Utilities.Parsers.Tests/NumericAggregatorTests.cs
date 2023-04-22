namespace CrossCutting.Utilities.Parsers.Tests;

public class NumericAggregatorTests
{
    [Fact]
    public void Evaluate_Returns_Invalid_When_Type_Is_Not_Supported()
    {
        // Act
        var result = NumericAggregator.Evaluate("apple", "pear"
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
}
