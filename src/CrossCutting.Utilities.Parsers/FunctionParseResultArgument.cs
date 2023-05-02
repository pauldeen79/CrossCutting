namespace CrossCutting.Utilities.Parsers;

public abstract record FunctionParseResultArgument
{
    public abstract Result<object?> GetValue(object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider);
}

public sealed record LiteralArgument : FunctionParseResultArgument
{
    public LiteralArgument(string value) => Value = value;

    public string Value { get; }

    public override Result<object?> GetValue(object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider)
    {
        var result = parser.Parse(Value, formatProvider, context);

        return result.Status == ResultStatus.NotSupported
            ? Result<object?>.Success(Value)
            : result;
    }
}

public sealed record FunctionArgument : FunctionParseResultArgument
{
    public FunctionArgument(FunctionParseResult function) => Function = function;

    public FunctionParseResult Function { get; }

    public override Result<object?> GetValue(object? context, IFunctionParseResultEvaluator evaluator, IExpressionParser parser, IFormatProvider formatProvider)
        => evaluator.Evaluate(Function, context);
}
