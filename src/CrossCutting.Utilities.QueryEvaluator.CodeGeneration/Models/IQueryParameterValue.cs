namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models;

internal interface IQueryParameterValue
{
    [Required] string Name { get; }
}
