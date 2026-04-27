namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IDelegateResultEvaluatable : IEvaluatable
{
    Func<Result<object?>> GetValue();
}