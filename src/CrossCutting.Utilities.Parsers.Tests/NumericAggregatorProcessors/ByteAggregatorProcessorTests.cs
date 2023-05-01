namespace CrossCutting.Utilities.Parsers.Tests.NumericAggregatorProcessors;

public class ByteAggregatorProcessorTests
{
    [Fact]
    public void Aggregate_Returns_Continue_When_FirstValue_Is_Not_Byte()
    {
        // Act
        var result = ByteAggregatorProcessor.Aggregate("no byte", (byte)2, CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
    }

    [Fact]
    public void Aggregate_Returns_Success_When_All_Is_Well()
    {
        // Act
        var result = ByteAggregatorProcessor.Aggregate((byte)2, (byte)3, CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(5);
    }
}
