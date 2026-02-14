namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface IInCondition : ICondition, ISourceExpressionContainer
{
    [Required][ValidateObject] IReadOnlyCollection<IEvaluatable> CompareExpressions { get; set; }
}
