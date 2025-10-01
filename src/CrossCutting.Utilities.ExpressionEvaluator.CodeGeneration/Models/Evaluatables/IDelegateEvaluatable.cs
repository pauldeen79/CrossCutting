namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface IDelegateEvaluatable : IEvaluatableBase
{
    Func<object?> Value { get; }
}
