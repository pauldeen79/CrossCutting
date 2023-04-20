namespace CrossCutting.Utilities.Parsers.Tests.NumericAggregatorProcessors;

public class Int64AggregatorProcessorTests
{
    [Fact]
    public void Aggregate_Returns_NotSupported_When_FirstValue_Is_Not_Int64()
    {
        // Act
        var result = new Int64AggregatorProcessor().Aggregate("no Int64", (long)2, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.NotSupported);
    }

    [Fact]
    public void Aggregate_Returns_Invalid_When_SecondValue_Is_Not_Int64()
    {
        // Act
        var result = new Int64AggregatorProcessor().Aggregate((long)2, "no Int64", (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Aggregate_Returns_Error_When_Aggregation_Fails()
    {
        // Act
        var result = new Int64AggregatorProcessor().Aggregate((long)2, (long)0, (_, _) => throw new InvalidOperationException("Kaboom"));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
    }

    [Fact]
    public void Aggregate_Returns_Success_When_All_Is_Well()
    {
        // Act
        var result = new Int64AggregatorProcessor().Aggregate((long)2, (long)3, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(5);
    }
}
