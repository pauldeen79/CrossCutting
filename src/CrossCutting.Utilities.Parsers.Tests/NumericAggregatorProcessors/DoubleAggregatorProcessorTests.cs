﻿namespace CrossCutting.Utilities.Parsers.Tests.NumericAggregatorProcessors;

public class DoubleAggregatorProcessorTests
{
    [Fact]
    public void Aggregate_Returns_Continue_When_FirstValue_Is_Not_Double()
    {
        // Act
        var result = DoubleAggregatorProcessor.Aggregate("no Double", (double)2, CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.ShouldBe(ResultStatus.Continue);
    }

    [Fact]
    public void Aggregate_Returns_Success_When_All_Is_Well()
    {
        // Act
        var result = DoubleAggregatorProcessor.Aggregate((double)2, (double)3, CultureInfo.InvariantCulture, (x, y) => x + y);

        // Assert
        result.Status.ShouldBe(ResultStatus.Ok);
        result.Value.ShouldBeEquivalentTo((double)5);
    }
}
