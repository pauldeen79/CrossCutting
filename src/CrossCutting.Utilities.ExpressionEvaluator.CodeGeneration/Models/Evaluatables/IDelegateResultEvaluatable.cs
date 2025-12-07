namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IDelegateResultEvaluatable : IEvaluatableBase
{
    Func<Result<object?>> Value { get; }
}

internal interface IDelegateResultEvaluatable<T> : IEvaluatableBase, IEvaluatable<T>
{
    Func<Result<T>> Value { get; }
}
