namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Conditions;

internal interface IInCondition : IConditionBase
{
    [Required][ValidateObject] IExpression SourceExpression { get; set; }
    [Required][ValidateObject] IReadOnlyCollection<IExpression> CompareExpressions { get; set; }
}
