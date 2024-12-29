namespace CrossCutting.Utilities.Parsers.FunctionCallArguments;

public partial record LiteralArgument
{
    public override Result<object?> GetValueResult(object? context, IFunctionEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider)
    {
        parser = ArgumentGuard.IsNotNull(parser, nameof(parser));

        var result = parser.Parse(Value, formatProvider, context);

        return result.Status == ResultStatus.NotSupported
            ? Result.Success<object?>(Value)
            : result;
    }

    public override Result ValidateValueResult(object? context, IFunctionEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider)
    {
        parser = ArgumentGuard.IsNotNull(parser, nameof(parser));

        var result = parser.Validate(Value, formatProvider, context);

        return result.Status == ResultStatus.NotSupported
            ? Result.Success()
            : result;
    }
}
