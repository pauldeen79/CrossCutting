namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface ILiteralResultEvaluatable : IEvaluatableBase
{
    Result<object?> Value { get; }
}

internal interface ILiteralResultEvaluatable<T> : IEvaluatableBase, IEvaluatable<T>
{
    Result<T> Value { get; }
}
