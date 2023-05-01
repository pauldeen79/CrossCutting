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
    public void Aggregate_Returns_Invalid_When_SecondValue_Is_Not_Byte()
    {
        // Act
        var result = ByteAggregatorProcessor.Aggregate((byte)2, "no byte", CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Aggregate_Returns_Error_When_Aggregation_Fails()
    {
        // Act
        var result = ByteAggregatorProcessor.Aggregate((byte)2, (byte)0, CultureInfo.InvariantCulture, (_, _) => throw new InvalidOperationException("Kaboom"));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
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
