namespace CrossCutting.Utilities.QueryEvaluator.Abstractions;

public interface IEvaluatableContext
{
    IEvaluatable<bool> Evaluatable { get; }
    Type EntityType { get; }
    object? Context { get; }
}