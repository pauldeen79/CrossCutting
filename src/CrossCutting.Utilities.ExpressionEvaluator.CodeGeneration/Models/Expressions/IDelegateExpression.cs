namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Expressions;

internal interface IDelegateExpression : IExpressionBase
{
    Func<object?> Value { get; }
}
