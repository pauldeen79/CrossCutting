namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Queries;

internal interface IParameterizedQuery : IQueryBase
{
    [Required][ValidateObject] IReadOnlyCollection<IQueryParameter> Parameters { get; }
}
