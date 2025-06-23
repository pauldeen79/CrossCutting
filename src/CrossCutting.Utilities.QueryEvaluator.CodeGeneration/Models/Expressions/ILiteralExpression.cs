namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Expressions;

internal interface ILiteralExpression : Abstractions.IExpression
{
    object? Value { get; }
}
