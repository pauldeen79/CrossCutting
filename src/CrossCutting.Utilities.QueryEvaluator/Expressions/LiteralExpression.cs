namespace CrossCutting.Utilities.QueryEvaluator.Expressions;

public partial record LiteralExpression
{
    public override Result<object?> Evaluate(object? context)
        => Result.Success(Value);
}
