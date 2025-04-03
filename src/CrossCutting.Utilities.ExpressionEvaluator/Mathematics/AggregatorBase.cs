namespace CrossCutting.Utilities.ExpressionEvaluator.Mathematics;

public abstract class AggregatorBase(char character, int order) : IAggregator
{
    public char Character { get; } = character;
    public int Order { get; } = order;

    public abstract Result<object?> Aggregate(object value1, object value2, IFormatProvider formatProvider);
    public Result<Type> Validate(Type type1, Type type2)
        => NumericAggregator.Validate(type1, type2);
}
