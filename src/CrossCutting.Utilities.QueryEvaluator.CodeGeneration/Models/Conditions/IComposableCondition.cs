namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Conditions;

internal interface IComposableCondition : ICondition
{
    StringComparison StringComparison { get; set; }

    Abstractions.IExpression LeftExpression { get; set; }
    IOperator Operator { get; set; }
    Abstractions.IExpression RightExpression { get; set; }
}
