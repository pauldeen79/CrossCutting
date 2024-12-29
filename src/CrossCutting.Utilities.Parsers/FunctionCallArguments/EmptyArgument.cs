namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record EmptyArgument
{
    public override Result<object?> GetValueResult(object? context, IFunctionEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider)
        => Result.Success(default(object?));

    public override Result ValidateValueResult(object? context, IFunctionEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider)
        => Result.Success();
}
