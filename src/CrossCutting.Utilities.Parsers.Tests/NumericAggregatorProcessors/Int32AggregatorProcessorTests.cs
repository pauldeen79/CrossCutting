namespace CrossCutting.Utilities.Parsers.Tests.NumericAggregatorProcessors;

public class Int32AggregatorProcessorTests
{
    [Fact]
    public void Aggregate_Returns_Continue_When_FirstValue_Is_Not_Int32()
    {
        // Act
        var result = Int32AggregatorProcessor.Aggregate("no Int32", 2, CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.ShouldBe(ResultStatus.Continue);
    }

    [Fact]
    public void Aggregate_Returns_Success_When_All_Is_Well()
    {
        // Act
        var result = Int32AggregatorProcessor.Aggregate(2, 3, CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo(5);
    }
}
