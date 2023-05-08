namespace CrossCutting.Utilities.Parsers;

public partial record FunctionParseResultArgument
{
    public abstract Result<object?> GetValueResult(object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider);
}
