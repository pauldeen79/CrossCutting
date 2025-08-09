namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Expressions;

internal interface IDelegateExpression : IExpressionBase
{
    Func<object?> Value { get; }
}
