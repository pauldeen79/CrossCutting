namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface ILiteralEvaluatable : IEvaluatable
{
    object? GetValue();
}