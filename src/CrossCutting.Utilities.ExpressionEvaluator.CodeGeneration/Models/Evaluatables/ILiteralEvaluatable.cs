namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Evaluatables;

internal interface ILiteralEvaluatable : IEvaluatableBase
{
    object? Value { get; }
}
