namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models;

internal interface IQuerySortOrder
{
    [Required][ValidateObject] IExpressionBase Expression { get; }
    QuerySortOrderDirection Order { get; }
}
