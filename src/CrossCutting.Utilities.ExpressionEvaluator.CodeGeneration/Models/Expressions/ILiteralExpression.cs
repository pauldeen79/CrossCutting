namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Expressions;

internal interface ILiteralExpression : IExpressionBase
{
    object? Value { get; }
}
