namespace CrossCutting.Utilities.Parsers.Tests.NumericAggregatorProcessors;

public class Int64AggregatorProcessorTests
{
    [Fact]
    public void Aggregate_Returns_Continue_When_FirstValue_Is_Not_Int64()
    {
        // Act
        var result = Int64AggregatorProcessor.Aggregate("no Int64", (long)2, CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.ShouldBe(ResultStatus.Continue);
    }

    [Fact]
    public void Aggregate_Returns_Success_When_All_Is_Well()
    {
        // Act
        var result = Int64AggregatorProcessor.Aggregate((long)2, (long)3, CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((long)5);
    }
}
