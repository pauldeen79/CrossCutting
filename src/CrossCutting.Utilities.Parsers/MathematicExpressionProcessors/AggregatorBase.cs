namespace CrossCutting.Utilities.Parsers.MathematicExpressionProcessors;

public abstract class AggregatorBase(char character, int order) : IAggregator
{
    public char Character { get; } = character;
    public int Order { get; } = order;

    public abstract Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider);
}
