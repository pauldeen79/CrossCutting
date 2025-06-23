namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface IQueryParameterValue
{
    [Required] string Name { get; }
}
