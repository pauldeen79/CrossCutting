namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

internal record AggregatorPosition
{
    internal char Character { get; }
    internal int Position { get; }

    public AggregatorPosition(char character, int position)
    {
        Character = character;
        Position = position;
    }
}
