namespace CrossCutting.Utilities.Parsers.Tests.NumericAggregatorProcessors;

public class Int16AggregatorProcessorTests
{
    [Fact]
    public void Aggregate_Returns_Continue_When_FirstValue_Is_Not_Int16()
    {
        // Act
        var result = new Int16AggregatorProcessor().Aggregate("no Int16", (short)2, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
    }

    [Fact]
    public void Aggregate_Returns_Invalid_When_SecondValue_Is_Not_Int16()
    {
        // Act
        var result = new Int16AggregatorProcessor().Aggregate((short)2, "no Int16", (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Aggregate_Returns_Error_When_Aggregation_Fails()
    {
        // Act
        var result = new Int16AggregatorProcessor().Aggregate((short)2, (short)0, (_, _) => throw new InvalidOperationException("Kaboom"));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
    }

    [Fact]
    public void Aggregate_Returns_Success_When_All_Is_Well()
    {
        // Act
        var result = new Int16AggregatorProcessor().Aggregate((short)2, (short)3, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(5);
    }
}
