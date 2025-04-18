namespace CrossCutting.Utilities.ExpressionEvaluator.Operators;

public class StringOperator : IOperator
{
    public string Value { get; }

    public StringOperator(string value)
    {
        ArgumentGuard.IsNotNull(value, nameof(value));

        Value = value;
    }

    public Result<object?> Evaluate(ExpressionEvaluatorContext context)
        => Result.Success<object?>(Value);

    public ExpressionParseResult Parse(ExpressionEvaluatorContext context)
        => new ExpressionParseResultBuilder()
            .WithSourceExpression($"\"{Value}\"")
            .WithExpressionType(typeof(StringOperator))
            .WithResultType(typeof(string))
            .WithStatus(ResultStatus.Ok);
}
