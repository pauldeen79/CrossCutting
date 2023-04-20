namespace CrossCutting.Utilities.Parsers.Tests.NumericAggregatorProcessors;

public class ByteAggregatorProcessorTests
{
    [Fact]
    public void Aggregate_Returns_NotSupported_When_FirstValue_Is_Not_Byte()
    {
        // Act
        var result = new ByteAggregatorProcessor().Aggregate("no byte", (byte)2, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.NotSupported);
    }

    [Fact]
    public void Aggregate_Returns_Invalid_When_SecondValue_Is_Not_Byte()
    {
        // Act
        var result = new ByteAggregatorProcessor().Aggregate((byte)2, "no byte", (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Aggregate_Returns_Error_When_Aggregation_Fails()
    {
        // Act
        var result = new ByteAggregatorProcessor().Aggregate((byte)2, (byte)0, (_, _) => throw new InvalidOperationException("Kaboom"));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
    }

    [Fact]
    public void Aggregate_Returns_Success_When_All_Is_Well()
    {
        // Act
        var result = new ByteAggregatorProcessor().Aggregate((byte)2, (byte)3, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(5);
    }
}
