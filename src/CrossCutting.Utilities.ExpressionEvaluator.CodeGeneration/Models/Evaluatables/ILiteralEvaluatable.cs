namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface ILiteralEvaluatable : IEvaluatableBase
{
    object? Value { get; }
}

internal interface ILiteralEvaluatable<T> : IEvaluatableBase, IEvaluatable<T>
{
    T Value { get; }
}
