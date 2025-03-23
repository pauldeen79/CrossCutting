namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IOperator : IBuildableEntity<IOperatorBuilder>
{
    int Order { get; }
    string OperatorExpression { get; }
    Result<bool> Evaluate(OperatorContext context);
}
