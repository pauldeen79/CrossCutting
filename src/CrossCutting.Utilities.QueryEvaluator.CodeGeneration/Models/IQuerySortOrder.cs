namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models;

internal interface IQuerySortOrder
{
    [Required][ValidateObject] IExpression Expression { get; }
    QuerySortOrderDirection Order { get; }
}
