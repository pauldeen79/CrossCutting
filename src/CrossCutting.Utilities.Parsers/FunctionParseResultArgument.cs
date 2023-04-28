namespace CrossCutting.Utilities.Parsers;

public abstract record FunctionParseResultArgument
{
    public abstract Result<object?> GetValue(object? context, IFunctionParseResultEvaluator evaluator);
}

public sealed record LiteralArgument : FunctionParseResultArgument
{
    public LiteralArgument(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public override Result<object?> GetValue(object? context, IFunctionParseResultEvaluator evaluator) => Result<object?>.Success(Value);
}

public sealed record FunctionArgument : FunctionParseResultArgument
{
    public FunctionArgument(FunctionParseResult function)
    {
        Function = function;
    }

    public FunctionParseResult Function { get; }

    public override Result<object?> GetValue(object? context, IFunctionParseResultEvaluator evaluator)
        => evaluator.Evaluate(Function, context);
}
