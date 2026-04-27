namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IDelegateEvaluatable : IEvaluatable
{
    Func<object?> GetValue();
}