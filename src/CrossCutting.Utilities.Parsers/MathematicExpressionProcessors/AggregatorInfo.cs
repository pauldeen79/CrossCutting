namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

internal sealed record AggregatorInfo
{
    public AggregatorBase Aggregator { get; }
    public int Index { get; }

    public AggregatorInfo(AggregatorBase aggregator, int index)
    {
        Aggregator = aggregator;
        Index = index;
    }
}
