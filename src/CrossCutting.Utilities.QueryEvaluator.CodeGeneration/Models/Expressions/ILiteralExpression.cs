namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Expressions;

internal interface ILiteralExpression : IExpressionBase
{
    object? Value { get; }
}
