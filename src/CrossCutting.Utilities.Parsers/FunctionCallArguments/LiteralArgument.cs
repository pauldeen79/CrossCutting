namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record LiteralArgument
{
    public override Result<object?> GetValueResult(object? context, IFunctionEvaluator functionEvaluator, IExpressionEvaluator expressionEvaluator, IFormatProvider formatProvider)
    {
        expressionEvaluator = ArgumentGuard.IsNotNull(expressionEvaluator, nameof(expressionEvaluator));

        var result = expressionEvaluator.Evaluate(Value, formatProvider, context);

        return result.Status == ResultStatus.NotSupported
            ? Result.Success<object?>(Value)
            : result;
    }
}
