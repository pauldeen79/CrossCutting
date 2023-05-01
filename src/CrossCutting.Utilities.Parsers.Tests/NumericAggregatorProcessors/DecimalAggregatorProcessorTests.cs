namespace CrossCutting.Utilities.Parsers.Tests.NumericAggregatorProcessors;

public class DecimalAggregatorProcessorTests
{
    [Fact]
    public void Aggregate_Returns_Continue_When_FirstValue_Is_Not_Decimal()
    {
        // Act
        var result = DecimalAggregatorProcessor.Aggregate("no Decimal", (decimal)2, CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
    }

    [Fact]
    public void Aggregate_Returns_Success_When_All_Is_Well()
    {
        // Act
        var result = DecimalAggregatorProcessor.Aggregate((decimal)2, (decimal)3, CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(5);
    }
}
