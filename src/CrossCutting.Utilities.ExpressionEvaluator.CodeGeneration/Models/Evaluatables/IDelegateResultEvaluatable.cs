namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IDelegateResultEvaluatable : IEvaluatableBase
{
    [CsharpTypeName("System.Func<CrossCutting.Common.Results.Result<object?>>")]
    Func<Result<object?>> Value { get; }
}

internal interface IDelegateResultEvaluatable<T> : IEvaluatableBase, IEvaluatable<T>
{
    Func<Result<T>> Value { get; }
}
