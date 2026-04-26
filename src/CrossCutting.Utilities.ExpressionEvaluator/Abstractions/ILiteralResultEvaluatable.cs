namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface ILiteralResultEvaluatable : IEvaluatable
{
    Result GetValue();
}