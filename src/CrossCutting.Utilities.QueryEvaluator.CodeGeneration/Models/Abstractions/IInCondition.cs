namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface IInCondition : ICondition
{
    [Required][ValidateObject] IEvaluatable SourceExpression { get; set; }
    [Required][ValidateObject] IReadOnlyCollection<IEvaluatable> CompareExpressions { get; set; }
}
