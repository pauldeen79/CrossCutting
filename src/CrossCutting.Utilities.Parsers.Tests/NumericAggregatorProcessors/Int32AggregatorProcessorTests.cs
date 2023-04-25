namespace CrossCutting.Utilities.Parsers.Tests.NumericAggregatorProcessors;

public class Int32AggregatorProcessorTests
{
    [Fact]
    public void Aggregate_Returns_Continue_When_FirstValue_Is_Not_Int32()
    {
        // Act
        var result = new Int32AggregatorProcessor().Aggregate("no Int32", 2, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
    }

    [Fact]
    public void Aggregate_Returns_Invalid_When_SecondValue_Is_Not_Int32()
    {
        // Act
        var result = new Int32AggregatorProcessor().Aggregate(2, "no Int32", (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Aggregate_Returns_Error_When_Aggregation_Fails()
    {
        // Act
        var result = new Int32AggregatorProcessor().Aggregate(2, 0, (_, _) => throw new InvalidOperationException("Kaboom"));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
    }

    [Fact]
    public void Aggregate_Returns_Success_When_All_Is_Well()
    {
        // Act
        var result = new Int32AggregatorProcessor().Aggregate(2, 3, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(5);
    }
}
