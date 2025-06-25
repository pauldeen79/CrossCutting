namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Expressions;

internal interface ILiteralExpression : IExpression
{
    object? Value { get; }
}
