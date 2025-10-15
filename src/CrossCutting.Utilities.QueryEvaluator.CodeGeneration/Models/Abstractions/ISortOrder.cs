namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface ISortOrder
{
    [Required][ValidateObject] IEvaluatable Expression { get; }
    SortOrderDirection Order { get; }
}
