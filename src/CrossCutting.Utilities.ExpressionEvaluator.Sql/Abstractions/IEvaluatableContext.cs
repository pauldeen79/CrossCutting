namespace CrossCutting.Utilities.QueryEvaluator.Abstractions;

public interface IEvaluatableContext
{
    IEvaluatable<bool> Evaluatable { get; }
    Type EntityType { get; }
    IEvaluatable? OrderByEvaluatable { get; }
    object? Context { get; }
}