namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface ISortOrder
{
    [Required][ValidateObject] IExpression Expression { get; }
    SortOrderDirection Order { get; }
}
