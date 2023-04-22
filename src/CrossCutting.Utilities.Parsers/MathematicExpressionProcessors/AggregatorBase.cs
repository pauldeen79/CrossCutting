namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

public abstract class AggregatorBase : IAggregator
{
    public char Character { get; }
    public int Order { get; }

    protected AggregatorBase(char character, int order)
    {
        Character = character;
        Order = order;
    }

    public abstract Result<object> Aggregate(object value1, object value2);
}
