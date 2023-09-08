namespace CrossCutting.Utilities.Parsers.FunctionParseResultArguments;

public partial record LiteralArgument
{
    public override Result<object?> GetValueResult(object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider)
    {
        parser = ArgumentGuard.IsNotNull(parser, nameof(parser));

        var result = parser.Parse(Value, formatProvider, context);

        return result.Status == ResultStatus.NotSupported
            ? Result<object?>.Success(Value)
            : result;
    }
}

public partial record LiteralArgumentBase
{
    public override Result<object?> GetValueResult(object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider)
        => throw new NotSupportedException();
}
