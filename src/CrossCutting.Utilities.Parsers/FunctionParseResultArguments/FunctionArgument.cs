namespace CrossCutting.Utilities.Parsers.FunctionParseResultArguments;

public partial record FunctionArgument
{
    public override Result<object?> GetValueResult(object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider)
        => ArgumentGuard.IsNotNull(evaluator, nameof(evaluator)).Evaluate(Function, parser, context);
}
