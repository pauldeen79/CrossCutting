namespace CrossCutting.Utilities.Parsers.Tests.NumericAggregatorProcessors;

public class SingleAggregatorProcessorTests
{
    [Fact]
    public void Aggregate_Returns_Continue_When_FirstValue_Is_Not_Single()
    {
        // Act
        var result = SingleAggregatorProcessor.Aggregate("no Single", (float)2, CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.ShouldBe(ResultStatus.Continue);
    }

    [Fact]
    public void Aggregate_Returns_Success_When_All_Is_Well()
    {
        // Act
        var result = SingleAggregatorProcessor.Aggregate((float)2, (float)3, CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((float)5);
    }
}
