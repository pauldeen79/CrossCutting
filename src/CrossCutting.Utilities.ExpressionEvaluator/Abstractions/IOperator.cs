namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public partial interface IOperator
{
    Result<bool> Evaluate(OperatorContext context);
}
