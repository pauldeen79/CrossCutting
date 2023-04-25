namespace CrossCutting.Utilities.Parsers.Tests.NumericAggregatorProcessors;

public class DecimalAggregatorProcessorTests
{
    [Fact]
    public void Aggregate_Returns_Continue_When_FirstValue_Is_Not_Decimal()
    {
        // Act
        var result = new DecimalAggregatorProcessor().Aggregate("no Decimal", (decimal)2, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Continue);
    }

    [Fact]
    public void Aggregate_Returns_Invalid_When_SecondValue_Is_Not_Decimal()
    {
        // Act
        var result = new DecimalAggregatorProcessor().Aggregate((decimal)2, "no Decimal", (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Invalid);
    }

    [Fact]
    public void Aggregate_Returns_Error_When_Aggregation_Fails()
    {
        // Act
        var result = new DecimalAggregatorProcessor().Aggregate((decimal)2, (decimal)0, (_, _) => throw new InvalidOperationException("Kaboom"));

        // Assert
        result.Status.Should().Be(ResultStatus.Error);
    }

    [Fact]
    public void Aggregate_Returns_Success_When_All_Is_Well()
    {
        // Act
        var result = new DecimalAggregatorProcessor().Aggregate((decimal)2, (decimal)3, (x, y) => x + y);

        // Assert
        result.Status.Should().Be(ResultStatus.Ok);
        result.Value.Should().BeEquivalentTo(5);
    }
}
