namespace CrossCutting.Utilities.Parsers;

public partial record FunctionCallArgument
{
    public abstract Result<object?> GetValueResult(object? context, IFunctionEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider);
}
