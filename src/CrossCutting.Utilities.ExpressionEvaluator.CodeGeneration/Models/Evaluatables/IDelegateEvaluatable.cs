namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IDelegateEvaluatable : IEvaluatableBase
{
    Func<object?> Value { get; }
}

internal interface IDelegateEvaluatable<T> : IEvaluatableBase, IEvaluatable<T>
{
    Func<T> Value { get; }
}
