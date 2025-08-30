namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Abstractions;

internal interface IContextualQuery : IQuery
{
    object? Context { get; }
}
