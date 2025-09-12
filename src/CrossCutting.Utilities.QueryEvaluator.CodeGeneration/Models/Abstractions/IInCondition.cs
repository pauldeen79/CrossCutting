namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface IInCondition : ICondition
{
    [Required][ValidateObject] IExpression SourceExpression { get; set; }
    [Required][ValidateObject] IReadOnlyCollection<IExpression> CompareExpressions { get; set; }
}
