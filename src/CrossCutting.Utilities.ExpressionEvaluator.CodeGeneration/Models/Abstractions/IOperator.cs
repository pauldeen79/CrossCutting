namespace CrossCutting.Utilities.ExpressionEvaluator.CodeGeneration.Models.Abstractions;

internal interface IOperator
{
    int Order { get; }
    [Required] string OperatorExpression { get; }
}
