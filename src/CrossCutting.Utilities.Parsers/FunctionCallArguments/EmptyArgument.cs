namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record EmptyArgument
{
    public override Result<object?> GetValueResult(object? context, IFunctionEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider)
        => Result.Success(default(object?));
}
