namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface IParameterizedQuery : IQuery
{
    [Required][ValidateObject] IReadOnlyCollection<IQueryParameter> Parameters { get; }
}
