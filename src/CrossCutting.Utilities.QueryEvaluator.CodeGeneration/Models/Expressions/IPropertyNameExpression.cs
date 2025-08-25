namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Expressions;

internal interface IPropertyNameExpression : IExpressionBase
{
    [Required] string PropertyName { get; }
    [Required] IExpression Expression { get; }
}
