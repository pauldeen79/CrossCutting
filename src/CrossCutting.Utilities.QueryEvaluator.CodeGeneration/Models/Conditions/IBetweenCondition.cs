namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Conditions;

internal interface IBetweenCondition : IConditionBase, ISourceExpressionContainer
{
    [Required][ValidateObject] IEvaluatable LowerBoundExpression { get; set; }
    [Required][ValidateObject] IEvaluatable UpperBoundExpression { get; set; }
}
