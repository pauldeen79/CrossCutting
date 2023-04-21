namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

public abstract class Aggregator
{
    public char Character { get; }
    public int Order { get; }

    protected Aggregator(char character, int order)
    {
        Character = character;
        Order = order;
    }

    public abstract Result<object> Aggregate(object value1, object value2);
}
