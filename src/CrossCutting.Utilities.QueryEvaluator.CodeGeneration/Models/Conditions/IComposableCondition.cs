namespace CrossCutting.Utilities.QueryEvaluator.CodeGeneration.Models.Conditions;

internal interface IComposableCondition : ICondition
{
    IExpression LeftExpression { get; set; }
    IOperator Operator { get; set; }
    IExpression RightExpression { get; set; }
}
