namespace CrossCutting.Utilities.Parsers.Tests.NumericAggregatorProcessors;

public class Int16AggregatorProcessorTests
{
    [Fact]
    public void Aggregate_Returns_Continue_When_FirstValue_Is_Not_Int16()
    {
        // Act
        var result = Int16AggregatorProcessor.Aggregate("no Int16", (short)2, CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
    }

    [Fact]
    public void Aggregate_Returns_Success_When_All_Is_Well()
    {
        // Act
        var result = Int16AggregatorProcessor.Aggregate((short)2, (short)3, CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(5);
    }
}
