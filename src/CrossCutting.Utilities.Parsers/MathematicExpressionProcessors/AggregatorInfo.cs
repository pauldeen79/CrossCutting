namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

internal sealed record AggregatorInfo
{
    public Aggregator Aggregator { get; }
    public int Index { get; }

    public AggregatorInfo(Aggregator aggregator, int index)
    {
        Aggregator = aggregator;
        Index = index;
    }
}
