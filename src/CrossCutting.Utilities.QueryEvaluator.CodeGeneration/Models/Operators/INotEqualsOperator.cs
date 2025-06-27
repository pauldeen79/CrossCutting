namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Operators;

internal interface INotEqualsOperator : IOperator
{
    StringComparison StringComparison { get; }
}
