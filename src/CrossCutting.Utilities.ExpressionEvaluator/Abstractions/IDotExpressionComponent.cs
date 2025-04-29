namespace CrossCutting.Utilities.ExpressionEvaluator.Abstractions;

public interface IDotExpressionComponent
{
    int Order { get; }
    Result<object?> Evaluate(DotExpressionComponentState state);
    Result<Type> Validate(DotExpressionComponentState state);
}
