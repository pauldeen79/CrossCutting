namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Conditions;

internal interface IBetweenCondition : IConditionBase
{
    [Required][ValidateObject] IEvaluatable SourceExpression { get; set; }
    [Required][ValidateObject] IEvaluatable LowerBoundExpression { get; set; }
    [Required][ValidateObject] IEvaluatable UpperBoundExpression { get; set; }
}
