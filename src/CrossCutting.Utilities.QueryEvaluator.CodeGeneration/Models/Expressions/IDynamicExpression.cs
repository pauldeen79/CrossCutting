namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Expressions;

internal interface IDynamicExpression : IExpressionBase
{
    [Required] IExpression Expression { get; }
}
