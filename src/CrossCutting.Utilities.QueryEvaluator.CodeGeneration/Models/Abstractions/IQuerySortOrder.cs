namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface IQuerySortOrder
{
    [Required][ValidateObject] IExpression Expression { get; }
    QuerySortOrderDirection Order { get; }
}
