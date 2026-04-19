namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IEvaluatableContext
{
    IEvaluatable<bool> Evaluatable { get; }
    Type EntityType { get; }
    IEvaluatable? OrderByEvaluatable { get; }
    object? Context { get; }
}