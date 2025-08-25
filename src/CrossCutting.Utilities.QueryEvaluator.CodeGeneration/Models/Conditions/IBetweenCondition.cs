namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Conditions;

internal interface IBetweenCondition : IConditionBase
{
    [Required][ValidateObject] IExpression SourceExpression { get; set; }
    [Required][ValidateObject] IExpression LowerBoundExpression { get; set; }
    [Required][ValidateObject] IExpression UpperBoundExpression { get; set; }
}
