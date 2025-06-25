namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Queries;

internal interface IParameterizedQuery : IQuery
{
    [Required][ValidateObject] IReadOnlyCollection<IQueryParameter> Parameters { get; }
}
